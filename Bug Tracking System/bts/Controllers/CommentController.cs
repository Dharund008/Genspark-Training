using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bts.Services;
using Bts.Models;
using Bts.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Bts.Hubs;
using Bts.Models.DTO;
using Microsoft.Extensions.Logging;
namespace Bts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<CommentController> _logger;

        public CommentController(ICommentService commentService, IHubContext<NotificationHub> hub,
            ILogger<CommentController> logger)
        {
            _commentService = commentService;
            _hubContext = hub;
            _logger = logger;
        }

        //[Authorize(Roles = "Tester,Developer,Admin")]
        [HttpPost("add-comment")]
        public async Task<IActionResult> AddComment([FromBody] CommentRequestDTO dto)
        {
            try
            {
                var userId = User.FindFirst("MyApp_Id")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("Unauthorized access attempt to AddComment");
                    return Unauthorized();
                }

                var comment = await _commentService.AddCommentAsync(dto, userId);
                await _hubContext.Clients.All
                    .SendAsync("ReceiveMessage", $"{userId} has commented for bug {comment.BugId} : {comment.Message}");
                _logger.LogInformation("User {UserId} added comment for bug {BugId}", userId, comment.BugId);
                return Ok(comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment");
                return BadRequest(ex.Message);
            }
        }

       // [Authorize(Roles = "Tester,Developer,Admin")]
        [HttpGet("bug/{bugId}")]
        public async Task<IActionResult> GetComments(int bugId)
        {
            var comments = await _commentService.GetCommentsByBugIdAsync(bugId);
            _logger.LogInformation("Retrieved comments for bug {BugId}", bugId);
            return Ok(comments);
        }

        //[Authorize(Roles = "Tester,Developer,Admin")]
        [HttpGet("comments-all")]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _commentService.GetAllComments();
            _logger.LogInformation("Retrieved all comments");
            return Ok(comments);
        }

        //[Authorize(Roles = "Tester,Developer,Admin")]
        [HttpGet("user-role/")]
        public async Task<IActionResult> GetCommentsByUserRole([FromQuery] string userRole)
        {
            var comments = await _commentService.GetCommentsByUserRole(userRole);
            _logger.LogInformation("Retrieved comments by user role {UserRole}", userRole);
            return Ok(comments);
        }
    }
}
