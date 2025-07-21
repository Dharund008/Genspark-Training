using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EventBookingApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            Log.Information("Logs Working");
            return Ok("Hello World");
        }
    }
}
