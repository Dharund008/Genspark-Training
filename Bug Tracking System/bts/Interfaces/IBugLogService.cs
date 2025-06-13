using Bts.Models;

namespace Bts.Interfaces
{
    public interface IBugLogService
    {
        Task LogEventAsync(int bugId, string action, string performedBy);
        Task<IEnumerable<BugLog>> GetAllBugLogsAsync(int page, int pagesize);

    }
}