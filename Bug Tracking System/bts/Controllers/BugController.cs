using Microsoft.AspNetCore.Mvc;
using Bts.Services;
using Bts.Models;
using Bts.Interfaces;
using Microsoft.AspNetCore.Authorization;

using Microsoft.Extensions.Logging;
namespace Bts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BugController : ControllerBase
    {
        private readonly IBugService _bugService;
        private readonly ILogger<BugController> _logger;

        public BugController(IBugService bugService, ILogger<BugController> logger)
        {
            _bugService = bugService;
            _logger = logger;
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
            [FromQuery] int pageSize = 7) //in paginated, u need to assign page again for next page
        {
            var bugs = await _bugService.GetAllBugsAsync(page, pageSize);
            _logger.LogInformation("Retrieved paginated bugs page {Page} with page size {PageSize}", page, pageSize);
            return Ok(bugs);
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
