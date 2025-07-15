using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bts.Services;
using Bts.Models;
using Bts.Interfaces;

using Microsoft.Extensions.Logging;
namespace Bts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statsService;

        public StatisticsController(IStatisticsService statsService)
        {
            _statsService = statsService;
        }

        [HttpGet("{role}")]
        public async Task<IActionResult> GetStats(string role)
        {
            var result = await _statsService.GetDashboardStatsAsync(role, User);
            return Ok(result);
        }
    }

}