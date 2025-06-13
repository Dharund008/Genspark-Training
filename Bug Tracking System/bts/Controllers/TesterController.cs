using Microsoft.AspNetCore.Mvc;
using Bts.Models;
using Bts.Models.DTO;
using Bts.Services;
using Bts.Interfaces;
using Bts.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

using Microsoft.Extensions.Logging;
namespace Bts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "TESTER")]
    public class TesterController : ControllerBase
    {
        private readonly ITesterService _testerService;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<TesterController> _logger;

        public TesterController(ITesterService testerService, IHubContext<NotificationHub> hub,
            ILogger<TesterController> logger)
        {
            _testerService = testerService;
            _hubContext = hub;
            _logger = logger;
        }

        [HttpPost("create-bug")]
        public async Task<IActionResult> CreateBug([FromBody] BugSubmissionDTO dto)
        {
            if (!Enum.IsDefined(typeof(BugPriority), dto.Priority))
            {
                _logger.LogWarning("Invalid priority value {Priority} in CreateBug", dto.Priority);
                return BadRequest("Invalid priority value.");
            }

            var CreatedBug = await _testerService.CreateBugAsync(dto);
            var testerId = User.FindFirst("MyApp_Id")?.Value;
            //notify
            await _hubContext.Clients.Group("ADMIN")
                    .SendAsync("ReceiveMessage", $"Tester {testerId} has reported a new bug {CreatedBug.Id} .(View in Bugs)");

            _logger.LogInformation("Tester {TesterId} created bug {BugId}", testerId, CreatedBug.Id);
            return Ok(CreatedBug);
        }

        [HttpPatch("update-bug-details")]
        public async Task<IActionResult> UpdateBug([FromQuery] int bugId, [FromBody] BugSubmissionDTO dto)
        {
            if (!Enum.IsDefined(typeof(BugPriority), dto.Priority))
            {
                _logger.LogWarning("Invalid priority value {Priority} in UpdateBug", dto.Priority);
                return BadRequest("Invalid priority value.");
            }

            var updatedBug = await _testerService.UpdateBugAsync(bugId, dto);
            var testerId = User.FindFirst("MyApp_Id")?.Value;
            await _hubContext.Clients.Group("ADMIN")
                    .SendAsync("ReceiveMessage", $"Tester {testerId} has updated a bug ({bugId})");
            await _hubContext.Clients.Group("DEVELOPER")
                    .SendAsync("ReceiveMessage", $"Tester {testerId} has updated a bug ({bugId})");
            _logger.LogInformation("Tester {TesterId} updated bug {BugId}", testerId, bugId);
            return Ok(updatedBug);
        }



        [HttpGet("my-bugs")]
        public async Task<IActionResult> GetMyBugs()
        {
            var testerId = User.FindFirst("MyApp_Id")?.Value;
            if (string.IsNullOrEmpty(testerId))
            {
                _logger.LogWarning("Unauthorized access attempt to GetMyBugs");
                return Unauthorized();
            }
            var bugs = await _testerService.GetMyReportedBugsAsync(testerId);
            _logger.LogInformation("Retrieved bugs for tester {TesterId}", testerId);
            return Ok(bugs);
        }


        [HttpPut("update-bug-status/{bugId}")]
        public async Task<IActionResult> UpdateBugStatus(int bugId, [FromQuery] BugStatus newStatus)
        {
            var result = await _testerService.UpdateBugStatusAsync(bugId, newStatus);
            if (!result)
            {
                _logger.LogWarning("Failed to update bug status {NewStatus} for bug {BugId}", newStatus, bugId);
                return BadRequest("Invalid bug or status");
            }
            var testerId = User.FindFirst("MyApp_Id")?.Value;
            await _hubContext.Clients.Group("ADMIN")
                    .SendAsync("ReceiveMessage", $"Tester {testerId} has updated the bug ({bugId}) status to {newStatus}");

            await _hubContext.Clients.Group("DEVELOPER")
                    .SendAsync("ReceiveMessage", $"Tester {testerId} has updated the bug ({bugId}) status to {newStatus}");


            await _hubContext.Clients.Group("TESTER")
                    .SendAsync("ReceiveMessage", $"Tester {testerId} has updated the bug ({bugId}) status to {newStatus}");

            _logger.LogInformation("Updated bug status {NewStatus} for bug {BugId} by tester {TesterId}", newStatus, bugId, testerId);
            return Ok("Bug status updated");
        }

        // [HttpPost("add-comment")]
        // public async Task<IActionResult> AddComment([FromBody] Comment comment)
        // {
        //     var added = await _testerService.AddCommentAsync(comment);
        //     return Ok(added);
        // }

        [HttpGet("tester-by-email")]
        public async Task<IActionResult> GetTesterByEmail([FromQuery] string email)
        {
            try
            {
                var tester = await _testerService.GetTesterByEmail(email);
                _logger.LogInformation("Retrieved tester by email {Email}", email);
                return Ok(tester);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Tester with email {Email} not found", email);
                return NotFound($"Tester with email '{email}' not found.");
            }
        }

        [HttpGet("all-testers")]
        public async Task<IActionResult> GetAllTesters()
        {
            var testers = await _testerService.GetAllTesters();
            _logger.LogInformation("Retrieved all testers");
            return Ok(testers);
        }

        [HttpGet("testers-associated-with-bugs")]
        public async Task<IActionResult> GetTestersAssociatedWithBugs()
        {
            var testers = await _testerService.GetTestersAssociatedWithBugs();
            _logger.LogInformation("Retrieved testers associated with bugs");
            return Ok(testers);
        }



        [HttpPost("upload-screenshot")]
        public async Task<IActionResult> UploadScreenshot(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("File not selected in UploadScreenshot");
                return BadRequest("File not selected");
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var savePath = Path.Combine("wwwroot/screenshots", fileName);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            _logger.LogInformation("Uploaded screenshot {FileName}", fileName);
            return Ok(new { url = $"/screenshots/{fileName}" });
        }


    }
}
