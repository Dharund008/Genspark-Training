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
    public class HRAdminController : ControllerBase
    {
        private readonly IHRService _hrService;

        public HRAdminController(IHRService hrService)
        {
            _hrService = hrService;
        }

        [HttpPost("hr-login")]
        public async Task<ActionResult<HRAdmin>> RegisterUser([FromBody] HRRequestDTO hr)
        {
            try
            {
                var result = await _hrService.AddHRadmin(hr);
                if (result != null)
                    return Created("", result);
                return BadRequest("Controller:-Unable to process Adding HR!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Controller:-Error during HR registration: {e.Message}");
                return BadRequest(e.Message);
            }
        } 
    }
}