using NotifyAPI.Interfaces;
using NotifyAPI.Models;
using NotifyAPI.Services;
using NotifyAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace NotifyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly NotifyAPI.Interfaces.IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<ActionResult<LoginResponse>> UserAuthenticate(Login loginRequest)
        {
            try
            {
                var result = await _authenticationService.UserLogin(loginRequest);
                if (result == null)
                {
                    Console.WriteLine("Cotroller :- Authentication failed: User not found or password mismatch");
                    return Unauthorized("Controller:-Authentication failed: User not found or password mismatch");
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Controller-Error during authentication: {e.Message}");
                return Unauthorized(e.Message);
            }
        }
    }
}