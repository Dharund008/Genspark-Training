
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Online.Models;
using Online.Models.DTO;
using Online.Interfaces;
using Online.Contexts;
using Online.Services;

namespace Online.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ITokenService _tokenService;
        private readonly ICurrentUserService _currentService;
        private readonly MigrationContext _context;

        public AuthenticationController(IAuthenticationService authenticationService, ITokenService tokenService, ICurrentUserService currentService, MigrationContext context)
        {
            _authenticationService = authenticationService;
            _tokenService = tokenService;
            _currentService = currentService;
            _context = context;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("Invalid Login request");
                }
                var result = await _authenticationService.UserLogin(user);
                if (result == null)
                {
                    return Unauthorized("Invalid username or password");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("Invalid Register request");
                }
                var res = await _authenticationService.CreateUserAsync(user);
                if (res == null)
                {
                    return BadRequest("Invalid sign up request");
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("my-details")]
        public async Task<IActionResult> GetMyDetails()
        {
            try
            {
                var userId = User.FindFirst("MyApp_Id")?.Value;
                var user = await _context.Users.SingleOrDefaultAsync(u => u.UserId == int.Parse(userId));
                if (user == null)
                {
                    return BadRequest("User not matching!");
                }
                return Ok(new { message = "User details retrieved successfully!", user });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}