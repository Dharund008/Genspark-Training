using Bts.Contexts;
using Bts.Models;
using Bts.Interfaces;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;
namespace Bts.Services
{
    public class BugLogService : IBugLogService
    {
        private readonly BugContext _context;
        private readonly ILogger<BugLogService> _logger;

        public BugLogService(BugContext context, ILogger<BugLogService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task LogEventAsync(int bugId, string action, string performedBy)
        {
            try
            {
                _context.BugLogs.Add(new BugLog { BugId=bugId, Action=action, PerformedBy=performedBy });
                await _context.SaveChangesAsync();
                _logger.LogInformation("Logged event for bug {BugId} action {Action} by {PerformedBy}", bugId, action, performedBy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging event for bug {BugId} action {Action} by {PerformedBy}", bugId, action, performedBy);
                throw;
            }
        }
        public async Task<IEnumerable<BugLog>> GetAllBugLogsAsync(int page, int pageSize)
        {
            if (page < 1 || pageSize < 1)
            {
                _logger.LogWarning("Invalid page {Page} or pageSize {PageSize} in GetAllBugLogsAsync", page, pageSize);
                throw new ArgumentException("Page number and page size must be greater than zero.");
            }
        
            var logs = await _context.BugLogs
                .OrderByDescending(b => b.PerformedAt) // Sort logs by newest first
                .Skip((page - 1) * pageSize) // Skip previous pages
                .Take(pageSize) // Limit results to the requested page
                .ToListAsync();
            _logger.LogInformation("Retrieved bug logs page {Page} with page size {PageSize}", page, pageSize);
            return logs;
        }


    }
}
