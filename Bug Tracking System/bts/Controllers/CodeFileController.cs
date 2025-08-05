using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bts.Interfaces;
using Bts.Services;
using Bts.Models;
using Microsoft.AspNetCore.SignalR;
using Bts.Hubs;
using Bts.Contexts;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;
using System.Security.Claims;
namespace Bts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CodeFileController : ControllerBase
    {
        private readonly BugContext _context;
        private readonly ILogger<CodeFileController> _logger;
        private readonly IDeveloperService _developerService;
        private readonly ICodeFileService _codeFileService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public CodeFileController(BugContext bugContext, ILogger<CodeFileController> logger, IDeveloperService developerService
                                        , IHubContext<NotificationHub> hubContext, ICodeFileService codeFileService)
        {
            _context = bugContext;
            _logger = logger;
            _developerService = developerService;
            _hubContext = hubContext;
            _codeFileService = codeFileService;
        }

        [Authorize(Roles = "DEVELOPER, TESTER")]
        [Consumes("multipart/form-data")]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("No file provided for upload.");
                return BadRequest("File is missing or empty.");
            }
            using var stream = file.OpenReadStream();

            await _codeFileService.UploadFile(stream, file.FileName, "codefiles");
            var UserId = User.FindFirst("MyApp_Id")?.Value;
            await _hubContext.Clients.All
                .SendAsync("ReceiveMessage", $"File {file.FileName} uploaded successfully by Developer:{UserId}");
            return Ok("File uploaded - " + file.FileName);
        }

        [Authorize]
        [HttpGet("downloadfile")]
        public async Task<ActionResult<Stream>> Download(string fileName)
        {
            var stream = await _codeFileService.DownloadFile(fileName, "codefiles");
            if (stream == null) 
                return NotFound();
            return File(stream, "application/octet-stream", fileName);
        }




        // [Authorize(Roles = "DEVELOPER, TESTER")]
        // [HttpPost("upload-code")]
        // public async Task<IActionResult> UploadCode(IFormFile file)
        // {
        //     if (file == null || file.Length == 0)
        //     {
        //         return BadRequest("File is missing or empty.");
        //     }

        //     var role = User.FindFirst(ClaimTypes.Role)?.Value;
        //     var currentId = User.FindFirst("MyApp_Id")?.Value;

        //     if (string.IsNullOrEmpty(currentId))
        //     {
        //         _logger.LogWarning("Unauthorized access attempt to UploadCode");
        //         return Unauthorized();
        //     }

        //     try
        //     {
        //         var fileUrl = await _developerService.UploadCodeAsync(file, currentId);

        //         switch (role)
        //         {
        //             case "DEVELOPER":
        //                 await _hubContext.Clients.Group("TESTER")
        //                     .SendAsync("ReceiveMessage", $"Code uploaded by developer {currentId}: {fileUrl}");

        //                 await _hubContext.Clients.Group("DEVELOPER")
        //                 .SendAsync("ReceiveMessage", $"Code uploaded for review by {currentId}!");

        //                 await _hubContext.Clients.Group("ADMIN")
        //                     .SendAsync("ReceiveMessage", $"Developer {currentId} uploaded code for review.");

        //                 _logger.LogInformation("Developer {DeveloperId} uploaded code: {FileUrl}", currentId, fileUrl);
        //                 break;

        //             case "TESTER":
        //                 await _hubContext.Clients.Group("DEVELOPER")
        //                     .SendAsync("ReceiveMessage", $"Tester {currentId} has submitted a defect document: {fileUrl}");

        //                 _logger.LogInformation("Tester {TesterId} submitted defect file: {FileUrl}", currentId, fileUrl);
        //                 break;

        //             default:
        //                 return Forbid("Role not supported for this operation.");
        //         }

        //         return Ok(new { url = fileUrl });
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error uploading code for {CurrentId}", currentId);
        //         return StatusCode(500, "Internal server error.");
        //     }
        // }


        [Authorize]
        [HttpGet("get-all-code-files")]
        public async Task<IActionResult> GetAllUploads([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var totalCount = await _context.UploadedFileLogs.CountAsync();

            var uploads = await _context.UploadedFileLogs
                .OrderByDescending(u => u.UploadedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            _logger.LogInformation("Retrieved uploaded files page {Page} with page size {PageSize}", page, pageSize);

            return Ok(new
            {
                total = totalCount,
                page,
                pageSize,
                uploads
            });
        }
        [Authorize]
        [HttpGet("filter-developers-filelogs")]
        public async Task<IActionResult> GetFilterDevelopers([FromQuery] string developerId)
        {
            var uploads = await _context.UploadedFileLogs
                .Where(up => up.DeveloperId == developerId)
                .ToListAsync();

            if (uploads == null || uploads.Count == 0)
            {
                _logger.LogWarning("No uploads found for developer ID {DeveloperId}", developerId);
                return NotFound();
            }

            _logger.LogInformation("Retrieved uploads for developer ID {DeveloperId}", developerId);
            return Ok(uploads);
        }

    }
}
