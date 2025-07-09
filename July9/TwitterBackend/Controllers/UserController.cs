using Twitter.Models;
using Twitter.Interfaces;
using Twitter.Repositories;
using Twitter.Services;
using Twitter.Contexts;
using Twitter.DTOs;
using Twitter.MISC;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Twitter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper; //using automapper inbuilt interface
        //IMapper is AutoMapper's main engine.
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("register")] //endpoint
        public IActionResult RegisterUser([FromBody] CreateUserDTO userdto)
        {
            // _userService.RegisterUser(user);
            // return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);

            try
            {
                if (userdto == null)
                {
                    return BadRequest("User data is null!");
                }
                var user = _mapper.Map<User>(userdto);
                _userService.RegisterUser(user);
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message }); //handling errors!
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<ResponseUserClass>> GetAllUsers()
        {
            var erm = _userService.GetAllUsers();
            if (!erm.Any())
            {
                return NoContent();
            }
            var dto = _mapper.Map<IEnumerable<ResponseUserClass>>(erm);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public ActionResult<ResponseUserClass> GetUserById(int id)
        {
            //return Ok(_userService.GetUserById(id));
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<ResponseUserClass>(user);
            return Ok(dto);
        }

        [HttpPut("UserUpdate")]
        public IActionResult UpdateUser([FromBody] UpdateUserDTO userdto)
        {
            try
            {
                var user = _userService.GetUserById(userdto.Id);
                if (user == null && userdto == null)
                {
                    return NotFound();
                }
                var dto = _mapper.Map(userdto, user); //saved from dto to var(entity)

                _userService.UpdateUser(dto);
                return Ok("User updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("UserDelete/{id}")] //parameter and endpoint
        public IActionResult RemoveUser([FromQuery] int id)
        {
            try
            {
                var user = _userService.GetUserById(id);
                if (user == null)
                {
                    return NotFound();
                }
                _userService.RemoveUser(id);
                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}