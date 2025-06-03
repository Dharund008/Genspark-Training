using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class SecureController : ControllerBase
{
    [Authorize]  // Requires authentication
    [HttpGet]
    public IActionResult GetSecureData()
    {
        return Ok(new { Message = "This is a secured endpoint! Only authorized users can access it." });
    }
}
