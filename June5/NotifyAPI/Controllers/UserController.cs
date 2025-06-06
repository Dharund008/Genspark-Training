using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NotifyAPI.Interfaces;
using NotifyAPI.Models.DTO;
using NotifyAPI.Models;
using NotifyAPI.Services;

namespace NotifyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("user-login")]
        public async Task<ActionResult<User>> RegisterUser([FromBody] UserRequestDTO user)
        {
            try
            {
                var result = await _userService.AddUser(user);
                if (result != null)
                    return Created("", result);
                return BadRequest("Controller:-Unable to process Adding user!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Controller:-Error during user registration: {e.Message}");
                return BadRequest(e.Message);
            }
        } 
    }
}