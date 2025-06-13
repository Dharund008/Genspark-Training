using Bts.Interfaces;
using Bts.Models.DTO;
using Bts.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bts.Contexts;
using Microsoft.EntityFrameworkCore;


using Microsoft.Extensions.Logging;
namespace Bts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly BugContext _bugContext;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticationService authenticationService, BugContext bugContext,
            ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _bugContext = bugContext;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> UserAuthenticate(Login loginRequest)
        {
            try
            {
                var result = await _authenticationService.UserLogin(loginRequest);
                if (result == null)
                {
                    _logger.LogWarning("Authentication failed: User not found or password mismatch");
                    return Unauthorized("Controller:-Authentication failed: User not found or password mismatch");
                }
                _logger.LogInformation("User authenticated successfully: {Username}", loginRequest.Username);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error during authentication");
                return Unauthorized(e.Message);
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _authenticationService.Logout(token);
            _logger.LogInformation("Logout successful");
            return Ok("Logout successful. Token invalidated.");
        }

        [Authorize]
        [HttpGet("my-details")]
        public async Task<IActionResult> MyDetails()
        {
            var userId = User.FindFirst("MyApp_Id")?.Value;
            var check = await _bugContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
            if (check == null)
            {
                _logger.LogWarning("User not found in MyDetails for user ID {UserId}", userId);
                return BadRequest("User not matching!");
            }
            _logger.LogInformation("Retrieved user details for user ID {UserId}", userId);
            return Ok(check);
        }
    }
}
