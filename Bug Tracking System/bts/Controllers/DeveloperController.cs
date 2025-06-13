using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bts.Interfaces;
using Bts.Services;
using Bts.Models;
using Bts.Contexts;
using Microsoft.AspNetCore.SignalR;
using Bts.Hubs;

using Microsoft.Extensions.Logging;
namespace Bts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "DEVELOPER")]
    public class DeveloperController : ControllerBase
    {
        private readonly IDeveloperService _developerService;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<DeveloperController> _logger;

        public DeveloperController(IDeveloperService developerService, IHubContext<NotificationHub> hub,
            ILogger<DeveloperController> logger)
        {
            _developerService = developerService;
            _hubContext = hub;
            _logger = logger;
        }

        [HttpGet("assigned-bugs")]
        public async Task<IActionResult> GetAssignedBugs()
        {
            var developerId = User.FindFirst("MyApp_Id")?.Value;
            if (string.IsNullOrEmpty(developerId))
            {
                _logger.LogWarning("Unauthorized access attempt to GetAssignedBugs");
                return Unauthorized();
            }

            var bugs = await _developerService.GetAssignedBugsAsync(developerId);
            _logger.LogInformation("Retrieved assigned bugs for developer {DeveloperId}", developerId);
            return Ok(bugs);
            //Developer can open that screenshot using baseUrl + ScreenshotUrl:
        }

        [HttpPut("update-bug-status")]
        public async Task<IActionResult> UpdateBugStatus([FromQuery] int bugId, [FromQuery] BugStatus newStatus)
        {
            // var developerId = User.FindFirst("sub")?.Value;
            // if (string.IsNullOrEmpty(developerId)) return Unauthorized();
            var result = await _developerService.UpdateBugStatusAsync(bugId, newStatus);
            if (!result)
            {
                _logger.LogWarning("Failed to update bug status for bug {BugId} to {NewStatus}", bugId, newStatus);
                return BadRequest("Invalid status or not authorized for this bug");
            }
            var developerId = User.FindFirst("MyApp_Id")?.Value;
            await _hubContext.Clients.Group("TESTER")
                    .SendAsync("ReceiveMessage", $"Developer {developerId} updated bug {bugId} status to {newStatus}");
            _logger.LogInformation("Bug status updated for bug {BugId} to {NewStatus} by developer {DeveloperId}", bugId, newStatus, developerId);
            return Ok("Bug status updated");
        }

        // [HttpPost("add-comment")]
        // public async Task<IActionResult> AddComment([FromBody] Comment comment)
        // {
        //     var added = await _developerService.AddCommentAsync(comment);
        //     return Ok(added);
        // }

        [HttpGet("developer-by-email/")]
        public async Task<IActionResult> GetDeveloperByEmail([FromQuery] string email)
        {
            try
            {
                var developer = await _developerService.GetDeveloperByEmail(email);
                _logger.LogInformation("Retrieved developer by email {Email}", email);
                return Ok(developer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving developer by email {Email}", email);
                return NotFound($"Developer with email '{email}' not found.");
            }
        }

        [HttpGet("all-developers")]
        public async Task<IActionResult> GetAllDevelopers()
        {
            var developers = await _developerService.GetAllDevelopers();
            _logger.LogInformation("Retrieved all developers");
            return Ok(developers);
        }

        [HttpGet("developer-with-bugs")]
        public async Task<IActionResult> GetDeveloperWithBugs()
        {
            var developersWithBugs = await _developerService.GetDeveloperWithBugs();
            _logger.LogInformation("Retrieved developers with bugs");
            return Ok(developersWithBugs);
        }

        [Authorize(Roles = "DEVELOPER")]
        [HttpPost("upload-code")]
        public async Task<IActionResult> UploadCode(IFormFile file)
        {
            var developerId = User.FindFirst("MyApp_Id")?.Value;
            if (string.IsNullOrEmpty(developerId))
            {
                _logger.LogWarning("Unauthorized access attempt to UploadCode");
                return Unauthorized();
            }

            try
            {
                var fileUrl = await _developerService.UploadCodeAsync(file, developerId);
                // Notify testers & admin
                await _hubContext.Clients.Group("DEVELOPER")
                    .SendAsync("ReceiveMessage", $"New code uploaded for review by {developerId}!");
                await _hubContext.Clients.Group("TESTER")
                    .SendAsync("ReceiveMessage", $"New code uploaded by {developerId}: {fileUrl}");
                await _hubContext.Clients.Group("ADMIN")
                    .SendAsync("ReceiveMessage", $"Developer {developerId} uploaded code for review.");
                _logger.LogInformation("Developer {DeveloperId} uploaded code: {FileUrl}", developerId, fileUrl);
                return Ok(new { url = fileUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading code for developer {DeveloperId}", developerId);
                return BadRequest(ex.Message);
            }
        }


    }
}
