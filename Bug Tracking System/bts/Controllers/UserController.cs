using Bts.Interfaces;
using Bts.Models.DTO;
using Bts.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bts.Hubs;
using Microsoft.AspNetCore.SignalR;

using Microsoft.Extensions.Logging;
namespace Bts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, IHubContext<NotificationHub> hub, ICurrentUserService currentUserService,
            ILogger<UserController> logger)
        {
            _userService = userService;
            _hubContext = hub;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            var token = await _userService.GeneratePasswordResetTokenAsync(dto.Email);
            if (token == null)
            {
                _logger.LogWarning("Password reset token generation failed: Email not found {Email}", dto.Email);
                return NotFound("Email not found");
            }

            _logger.LogInformation("Password reset token generated for email {Email}", dto.Email);
            // In production: send token via email
            return Ok(new { token, message = "Use this token to reset your password." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            var result = await _userService.ResetPasswordAsync(dto);
            if (!result)
            {
                _logger.LogWarning("Password reset failed: Invalid or expired token");
                return BadRequest("Invalid or expired token");
            }
            //notify
            await _hubContext.Clients.Group("ADMIN")
                    .SendAsync("ReceiveMessage", $"{_currentUserService.Id} has reseted their password!");

            _logger.LogInformation("Password reset successful for user {UserId}", _currentUserService.Id);
            return Ok("Password reset successful.");
        }

    }
}
