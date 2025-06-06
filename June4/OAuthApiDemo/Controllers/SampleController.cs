using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OAuth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SampleController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult GetPublic()
        {
            return Ok("This is a public endpoint-accessible to everyone.");
        }

        [HttpGet("protected")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetSecured()
        {
            Console.WriteLine("✅ User is authorized and reached the controller.");
            return Ok("This is OAuth endpoint-accessible only to authenticated users.");
        }
    }
}