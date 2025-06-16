using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bts.Interfaces;
using Bts.Services;
using Bts.Models;
using Bts.Models.DTO;
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
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<DeveloperController> _logger;

        public DeveloperController(IDeveloperService developerService, IHubContext<NotificationHub> hub,
            ILogger<DeveloperController> logger, ICurrentUserService currentUserService)
        {
            _developerService = developerService;
            _hubContext = hub;
            _logger = logger;
            _currentUserService = currentUserService;
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

        [HttpGet("my-bugs-testers")]
        [Authorize(Roles = "DEVELOPER")]
        public async Task<IActionResult> GetTestersForMyBugs()
        {
            // Get developerId from current user context/service
            var developerId = _currentUserService.Id;
            var testers = await _developerService.GetTestersForMyBugsAsync(developerId);
            return Ok(testers);
        }


        [HttpGet("developer-with-bugs")]
        public async Task<IActionResult> GetDeveloperWithBugs()
        {
            var developersWithBugs = await _developerService.GetDeveloperWithBugs();
            _logger.LogInformation("Retrieved developers with bugs");
            return Ok(developersWithBugs);
        }

    }
}
