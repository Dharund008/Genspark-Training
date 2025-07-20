using Microsoft.AspNetCore.Mvc;
using Bts.Services;
using Bts.Models;
using Bts.Interfaces;
using Bts.Hubs;
using Bts.Contexts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
namespace Bts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BugController : ControllerBase
    {
        private readonly IBugService _bugService;
        private readonly BugContext _context;
        private readonly ILogger<BugController> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public BugController(IBugService bugService, ILogger<BugController> logger,
                IHubContext<NotificationHub> hubContext, BugContext context)
        {
            _bugService = bugService;
            _logger = logger;
            _hubContext = hubContext;
            _context = context;
        }


        [HttpPut("update-bug-status/{bugId}")]
        public async Task<IActionResult> UpdateBugStatus(int bugId, [FromQuery] BugStatus newStatus)
        {
            var result = await _bugService.UpdateBugStatusAsync(bugId, newStatus);
            if (!result)
            {
                _logger.LogWarning("Failed to update bug status {NewStatus} for bug {BugId}", newStatus, bugId);
                return BadRequest("Invalid bug or status");
            }
            var UserId = User.FindFirst("MyApp_Id")?.Value;
            var bugDetails = await _bugService.GetBugByIdAsync(bugId);

            if (!string.IsNullOrEmpty(bugDetails.AssignedTo))
            {
                await _hubContext.Clients.Group(bugDetails.AssignedTo)
                    .SendAsync("ReceiveMessage", 
                        $"Bug ({bugId} : {bugDetails.Title}) status changed to {newStatus} by {UserId}");
            }

            if (!string.IsNullOrEmpty(bugDetails.CreatedBy))
            {
                await _hubContext.Clients.Group(bugDetails.CreatedBy)
                    .SendAsync("ReceiveMessage", 
                        $"Bug ({bugId} : {bugDetails.Title}) status changed to {newStatus} by {UserId}");
            }
            await _hubContext.Clients.Group("ADMIN")
                    .SendAsync("ReceiveMessage", $"{UserId} has updated the bug ({bugId}) status to {newStatus}");

            

            _logger.LogInformation("Updated bug status {NewStatus} for bug {BugId} by tester {TesterId}", newStatus, bugId, UserId);
            //return Ok("Bug status updated");
            return Ok(new { message = "Bug status updated" });
        }

        [HttpGet("bug-id/{id}")]
        public async Task<IActionResult> GetBug(int id)
        {
            var bug = await _bugService.GetBugByIdAsync(id);
            if (bug == null)
            {
                _logger.LogWarning("Bug not found with ID {BugId} in GetBug", id);
                return NotFound("Bug not found");
            }
            _logger.LogInformation("Retrieved bug with ID {BugId}", id);
            return Ok(bug);
        }

        [HttpGet("unassigned")]
        [Authorize(Roles = "ADMIN,TESTER")]
        public async Task<IActionResult> GetUnassignedBugs()
        {
            var bugs = await _bugService.GetUnassignedBugsAsync();
            _logger.LogInformation("Retrieved unassigned bugs. Count: {Count}", bugs.Count());
            return Ok(bugs);
        }

        [HttpGet("paginated-bugsall")]
        public async Task<ActionResult<IEnumerable<Bug>>> GetAllBugs(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 4) //in paginated, u need to assign page again for next page
        {
            var total = await _context.Bugs.CountAsync();
            var bugs = await _bugService.GetAllBugsAsync(page, pageSize);
            _logger.LogInformation("Retrieved paginated bugs page {Page} with page size {PageSize}", page, pageSize);
            return Ok(new
            {
                page,
                pageSize,
                total,
                bugs
            });
        }

        // [HttpGet("all")] public async Task<ActionResult<IEnumerable<Bug>>> GetAllBugs() 
        // { var bugs = await _bugService.GetAllBugsAsync(); return Ok(bugs); }


        [HttpGet("Bug-by-status")]
        public async Task<ActionResult<IEnumerable<Bug>>> GetBugsByStatus([FromQuery] string status)
        {
            var bugs = await _bugService.GetBugsByStatus(status);
            _logger.LogInformation("Retrieved bugs by status {Status}", status);
            return Ok(bugs);
        }

        [HttpGet("bug-tester/")]
        public async Task<ActionResult<IEnumerable<Bug>>> GetBugsByTesterId([FromQuery] string testerId)
        {
            var bugs = await _bugService.GetBugsByTesterId(testerId);
            _logger.LogInformation("Retrieved bugs by tester ID {TesterId}", testerId);
            return Ok(bugs);
        }

        [HttpGet("bug-developer/")]
        public async Task<ActionResult<IEnumerable<Bug>>> GetBugsByDeveloperId([FromQuery] string developerId)
        {
            var bugs = await _bugService.GetBugsByDeveloperId(developerId);
            _logger.LogInformation("Retrieved bugs by developer ID {DeveloperId}", developerId);
            return Ok(bugs);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBugs([FromQuery] string searchTerm)
        {
            var bugs = await _bugService.SearchBugs(searchTerm);
            _logger.LogInformation("Searched bugs with term {SearchTerm}", searchTerm);
            return Ok(bugs);
        }


    }
}
