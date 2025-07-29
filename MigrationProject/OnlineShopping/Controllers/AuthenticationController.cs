
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

        public AuthenticationController(IAuthenticationService authenticationService, ITokenService tokenService)
        {
            _authenticationService = authenticationService;
            _tokenService = tokenService;
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
    }
}