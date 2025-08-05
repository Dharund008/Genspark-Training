using Microsoft.AspNetCore.Mvc;
using Bts.Models.DTO;
using Bts.Interfaces;
using Bts.Services;
using Bts.Models;
using Bts.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Bts.Contexts;

namespace Bts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IBugService _bugService;
         private readonly IHubContext<NotificationHub> _hubContext;
         private readonly BugContext _context;
         private readonly ILogger<TesterController> _logger;

        public AdminController(IAdminService adminService, IHubContext<NotificationHub> hub, BugContext context, ILogger<TesterController> logger,
            IBugService bugService)
        {
            _adminService = adminService;
            _hubContext = hub;
            _context = context;
            _logger = logger;
            _bugService = bugService;
        }

        [HttpPost("Add-Admin")]
        public async Task<IActionResult> AddAdmin(AdminRequestDTO request)
        {
            try
            {
                _logger.LogInformation("AddAdmin called with email: {Email}", request.Email);
                var admin = await _adminService.AddAdmin(request);
                
                if (admin != null)
                {
                    _logger.LogInformation("Admin added successfully: {Email}", admin.Email);
                    return Ok(new { message = "Admin added successfully", admin });
                }
                else
                {
                    _logger.LogWarning("Attempt to add admin failed. Email already exists: {Email}", request.Email);
                    return BadRequest(new { error = "Admin with this email already exists" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding admin: {Email}", request.Email);
                return StatusCode(500, new { error = "An internal server error occurred, please try again later." });
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("add-developer")]
        public async Task<IActionResult> AddDeveloper([FromBody] DeveloperRequestDTO dto)
        {
            try
            {
                _logger.LogInformation("Adding developer with email: {Email}", dto.Email);
                var developer = await _adminService.AddDeveloper(dto);

                if (developer != null)
                {
                    _logger.LogInformation("Developer added successfully: {Email}", developer.Email);
                    return Ok(new { message = "Developer added successfully", developer });
                }
                var UserId = User.FindFirst("MyApp_Id")?.Value;
                await _hubContext.Clients.Group("ADMIN")
                    .SendAsync("ReceiveMessage", $"{UserId} has added the Developer ({developer.Id} : {developer.Name}).");

                _logger.LogWarning("Developer addition failed.");
                return BadRequest(new { error = "Failed to add developer" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding developer.");
                return StatusCode(500, new { error = "Internal server error, please try again later." });
            }
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost("add-tester")]
        public async Task<IActionResult> AddTester([FromBody] TesterRequestDTO dto)
        {
            try
            {
                _logger.LogInformation("Adding tester with email: {Email}", dto.Email);
                var tester = await _adminService.AddTester(dto);

                if (tester != null)
                {
                    _logger.LogInformation("Tester added successfully: {Email}", dto.Email);
                    return Ok(new { message = "Tester added successfully", tester });
                }
                var UserId = User.FindFirst("MyApp_Id")?.Value;
                await _hubContext.Clients.Group("ADMIN")
                    .SendAsync("ReceiveMessage", $"{UserId} has added the Tester ({tester.Id} : {tester.Name}).");
                _logger.LogWarning("Tester addition failed.");
                return BadRequest(new { error = "Failed to add tester" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding tester.");
                return StatusCode(500, new { error = "Internal server error, please try again later." });
            }
        }


        [Authorize(Roles = "ADMIN")]
        [HttpPut("assign-bug")]
        public async Task<IActionResult> AssignBug([FromQuery] int bugId, [FromQuery] string developerId)
        {
            try
            {
                _logger.LogInformation("AssignBug called for BugId: {BugId} to DeveloperId: {DeveloperId}", bugId, developerId);
                var result = await _adminService.AssignBugToDeveloperAsync(bugId, developerId);
                if (!result)
                {
                    _logger.LogWarning("Bug not found for assignment: {BugId}", bugId);
                    return NotFound("Bug not found");
                }
                var bugDetails = await _bugService.GetBugByIdAsync(bugId);
                await _hubContext.Clients.Group("DEVELOPER")
                    .SendAsync("ReceiveMessage", $"Admin has assigned a new bug to you. Bug ID: {bugId}");

                if (!string.IsNullOrEmpty(bugDetails.CreatedBy))
                {
                    await _hubContext.Clients.Group(bugDetails.CreatedBy)
                        .SendAsync("ReceiveMessage", 
                            $"Admin has assigned a New bug to {developerId}, Bug ID: {bugId} : {bugDetails.Title}");
                }
                _logger.LogInformation("Bug {BugId} assigned to developer {DeveloperId}", bugId, developerId);

                return Ok(new { message = "Bug assigned successfully.", result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while assigning bug.");
                return StatusCode(500, new { error = "Internal server error, please try again later." });
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("close-bug/{bugId}")]
        public async Task<IActionResult> CloseBug(int bugId)
        {
            try
            {
                var result = await _adminService.CloseBugAsync(bugId);
                if (!result) return NotFound(new { error = "Bug not found." });

                await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"Admin has changed status of Bug ID: {bugId} to closed.");

                return Ok(new { message = "Bug closed successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while closing bug.");
                return StatusCode(500, new { error = "Internal server error, please try again later." });
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("delete-developer/")]
        public async Task<IActionResult> DeleteDeveloper([FromQuery] string developerId)
        {
            var ok = await _adminService.DeleteDeveloperAsync(developerId);
            if (ok)
            {
                await _hubContext.Clients.Group("ADMIN")
                    .SendAsync("ReceiveMessage", $"Admin has deleted a developer {developerId}");
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("delete-tester/")]
        public async Task<IActionResult> DeleteTester([FromQuery] string testerId) {
            var ok = await _adminService.DeleteTesterAsync(testerId);
            if (ok)
            {
                await _hubContext.Clients.Group("ADMIN")
                    .SendAsync("ReceiveMessage", $"Admin has deleted a tester {testerId}");
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }


        [Authorize(Roles = "ADMIN")]
        [HttpDelete("delete-bug/{bugId}")]
        public async Task<IActionResult> DeleteBug(int bugId)
        {
            try
            {
                 _logger.LogInformation("DeleteBug called for BugId: {BugId}", bugId);
                var result = await _adminService.DeleteBugAsync(bugId);
                 if (!result)
                {
                    _logger.LogWarning("Bug not found for deletion: {BugId}", bugId);
                    return NotFound("Bug not found.");
                }
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"Bug ID: {bugId} has been deleted by Admin!.");

                _logger.LogInformation("Bug {BugId} deleted by admin", bugId);
                return Ok(new { message = "Bug deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting bug.");
                return StatusCode(500, new { error = "Internal server error, please try again later." });
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("available-developers")]
        public async Task<IActionResult> GetAvailableDevelopers()
        {
            var developers = await _adminService.GetAvailableDevelopersAsync();
            return Ok(developers);
        }
        
        [Authorize(Roles = "ADMIN")]
        [HttpGet("check-user/{Email}")]
        public async Task<IActionResult> CheckUser(string Email)
        {
            try
            {
                _logger.LogInformation("CheckUser called for email: {Email}", Email);
                var userExists = await _adminService.IsEmailExists(Email);
                if (userExists != false)
                {
                    _logger.LogInformation("User exists: {Email}", Email);
                    return Ok(true);
                }
                _logger.LogWarning("User not found: {Email}", Email);
                return NotFound(new { error = "User not found." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while checking user existence.");
                return StatusCode(500, new { error = "Internal server error, please try again later." });
            }
        }


        [Authorize(Roles = "ADMIN")]
        [HttpGet("list-users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                _logger.LogInformation("GetUsers called");
                var users = await _adminService.GetAllUsersAsync();
                _logger.LogInformation("Users fetched successfully. Count: {Count}", users.Count());
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching user list.");
                return StatusCode(500, new { error = "Internal server error, please try again later." });
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("Every-developers")]
        public async Task<IActionResult> GetAllDevelopersWithDeleted()
        {
            var developers = await _adminService.GetAllDevelopersWithDeletedAsync();
            return Ok(developers);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("Every-testers")]
        public async Task<IActionResult> GetAllTestersWithDeleted()
        {
            var testers = await _adminService.GetAllTestersWithDeletedAsync();
            return Ok(testers);
        }


        [Authorize(Roles = "ADMIN")]
        [HttpGet("all-bugs")]
        public async Task<IActionResult> GetAllBugs()
        {
            _logger.LogInformation("GetAllBugs called");
            var page = 1;
            var pageSize = 3;
            var uploads = await _context.Bugs
                .OrderByDescending(u => u.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

             _logger.LogInformation("Bugs fetched successfully. Count: {Count}", uploads.Count);

            return Ok(new
            {
                page,
                pageSize,
                uploads
            });
        }


        [Authorize(Roles = "ADMIN")]
        [HttpGet("all-tester-bugs")]
        public async Task<IActionResult> GetAllTesterBugs([FromQuery] string testerId)
        {
            _logger.LogInformation("GetAllTesterBugs called for TesterId: {TesterId}", testerId);
            var bugs = await _adminService.GetAllBugsTesterAsync(testerId);
            _logger.LogInformation("Tester bugs fetched. Count: {Count}", bugs.Count());
            return Ok(bugs);
        }
        

        [Authorize(Roles = "ADMIN")]
        [HttpGet("all-developer-bugs")]
        public async Task<IActionResult> GetAllDeveloperBugs([FromQuery] string developerId)
        {
             _logger.LogInformation("GetAllDeveloperBugs called for DeveloperId: {DeveloperId}", developerId);
            var bugs = await _adminService.GetAllBugsDeveloperAsync(developerId);
            _logger.LogInformation("Developer bugs fetched. Count: {Count}", bugs.Count());
            return Ok(bugs);
        }


    }
}
