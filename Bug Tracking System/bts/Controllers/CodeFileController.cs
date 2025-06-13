using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bts.Interfaces;
using Bts.Services;
using Bts.Models;
using Bts.Contexts;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;
namespace Bts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CodeFileController : ControllerBase
    {
        private readonly BugContext _context;
        private readonly ILogger<CodeFileController> _logger;

        public CodeFileController(BugContext bugContext, ILogger<CodeFileController> logger)
        {
            _context = bugContext;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("get-all-code-files")]
        public async Task<IActionResult> GetAllUploads([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var totalCount = await _context.UploadedFileLogs.CountAsync();

            var uploads = await _context.UploadedFileLogs
                .OrderByDescending(u => u.UploadedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            _logger.LogInformation("Retrieved uploaded files page {Page} with page size {PageSize}", page, pageSize);

            return Ok(new
            {
                total = totalCount,
                page,
                pageSize,
                uploads
            });
        }
        [Authorize]
        [HttpGet("filter-developers-filelogs")]
        public async Task<IActionResult> GetFilterDevelopers([FromQuery] string developerId)
        {
            var uploads = await _context.UploadedFileLogs
                .Where(up => up.DeveloperId == developerId)
                .ToListAsync();

            if (uploads == null || uploads.Count == 0)
            {
                _logger.LogWarning("No uploads found for developer ID {DeveloperId}", developerId);
                return NotFound();
            }

            _logger.LogInformation("Retrieved uploads for developer ID {DeveloperId}", developerId);
            return Ok(uploads);
        }
    }
}
