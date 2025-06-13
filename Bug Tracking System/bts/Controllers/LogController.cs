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
    public class LogController : ControllerBase
    {
        private readonly IBugLogService _bugLogService;
        private readonly ILogger<LogController> _logger;

        public LogController(IBugLogService bugLogService, ILogger<LogController> logger)
        {
            _bugLogService = bugLogService;
            _logger = logger;
        }


        [Authorize(Roles = "ADMIN, TESTER, DEVELOPER")]
        [HttpGet("buglogs")]
        public async Task<ActionResult<IEnumerable<BugLog>>> GetLogs([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var logs = await _bugLogService.GetAllBugLogsAsync(page, pageSize);
            _logger.LogInformation("Retrieved bug logs page {Page} with page size {PageSize}", page, pageSize);
            return Ok(logs);
        }
    }
}
