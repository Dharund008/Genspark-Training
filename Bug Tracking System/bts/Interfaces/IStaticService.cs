using System.Security.Claims;

namespace Bts.Interfaces
{
        public interface IStatisticsService
    {
        Task<object> GetDashboardStatsAsync(string role, ClaimsPrincipal user);
    }
}