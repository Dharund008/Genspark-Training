using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet("index")]
        public IActionResult Index()
        {
            return Ok("Home Page for sample API");
        }

        [HttpGet("about")]
        public IActionResult About()
        {
            var res = "Your application description page";
            return Ok(res);
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
            var res = "Your contact page";
            return Ok(res);
        }
    }

}