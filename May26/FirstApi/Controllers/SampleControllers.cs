using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

[ApiController]

[Route("/api/[controller]")]

public class SampleController : ControllerBase
{
    // [HttpGet] --this used to map to gets request method
    // public string GetGreet()
    // {
    //     return "HelloWorld";
    // }
    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)] // return type of the method
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    //producerResponseType - documents the return responses of the method. It is used by the client to know what to expect from the server.
    //either the 200 or 404 status code will be returned.
    
    
    public ActionResult GetGreet()
    {
        return Ok("Hello World");
    }
}

