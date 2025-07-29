using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly MigrationContext _context;

        public UserController(IUserService userService, MigrationContext context)
        {
            _userService = userService;
            _context = context;
        }

        [HttpGet("check-username")]
        public async Task<IActionResult> CheckUsername([FromQuery] string username)
        {
            try
            {
                var result = await _userService.IsUsernameExists(username);
                if (result)
                {
                    return Ok(new { message = "User exists!", result });
                }
                return BadRequest(new { error = "Failed to check User!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in UserController : UsernameCheck, please try again later." });
            }
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            try
            {
                var res = await _userService.GetByIdAsync(id);
                if (res != null)
                {
                    return Ok(new { message = "User found!", res });
                }
                return NotFound(new { error = "User not found!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in UserController : getById, please try again later." });
            }
        }

        [HttpPatch("change-username")]
        public async Task<IActionResult> ChangeUsername([FromQuery] int id, [FromBody] UsernameDTO username)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user != null)
                {
                    var updateName = await _userService.ChangeUsernameAsync(id, username);
                    if (updateName != null)
                    {
                        return Ok(new { message = "Username updated successfully!" });
                    }
                }
                return BadRequest(new { error = "Failed to update Username!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in UserController : ChangeUsername, please try again later." });
            }
        }

        [HttpPatch("change-phoneNumber")]
        public async Task<IActionResult> ChangePhoneNumber([FromQuery] int id, [FromBody] PhoneDTO phone)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user != null)
                {
                    var updateName = await _userService.ChangePhoneAsync(id, phone);
                    if (updateName != null)
                    {
                        return Ok(new { message = "PhoneNumber updated successfully!" });
                    }
                }
                return BadRequest(new { error = "Failed to update PhoneNumber!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in UserController : ChangePhone, please try again later." });
            }
        }

        [HttpPatch("change-address")]
        public async Task<IActionResult> ChangeAddress([FromQuery] int id, [FromBody] AddressDTO address)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user != null)
                {
                    var updateName = await _userService.ChangeAddressAsync(id, address);
                    if (updateName != null)
                    {
                        return Ok(new { message = "Address updated successfully!" });
                    }
                }
                return BadRequest(new { error = "Failed to update Address!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in UserController : ChangeAddress, please try again later." });
            }
        }

        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetALUsers()
        {
            try
            {
                var res = await _userService.GetAllAsync();
                if (res != null)
                {
                    return Ok(new { message = "Users found!", res });
                }
                return NotFound(new { error = "Users not found!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in UserController : GetAllUsers , please try again later." });
            }
        }
    }
}