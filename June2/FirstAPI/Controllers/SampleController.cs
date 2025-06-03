using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("/api/[controller]")]
public class SampleController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]

    [Authorize(Roles ="Doctor")]
    public ActionResult GetGreet()
    {
        return Ok("Hello World");
    }

    
    
}