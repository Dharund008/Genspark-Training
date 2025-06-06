using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NotifyAPI.Interfaces;
using NotifyAPI.Models.DTO;
using NotifyAPI.Models;
using NotifyAPI.Services;

[ApiController]
[Route("/api/[controller]")]
public class SampleController : ControllerBase
{
    [HttpGet("greet")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]

    [Authorize(Roles = "HR")]
    public ActionResult GetGreet()
    {
        return Ok("Hello HR");
    }

    [HttpGet("greet2")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [Authorize(Roles = "User")]
    public ActionResult GetGreet2()
    {
        return Ok("Hello User");

    }
}