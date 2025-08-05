using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Bts.Interfaces;
using Bts.Contexts;
using Bts.Models;

namespace Bts.Services
{
    public class StatisticsService : IStatisticsService
    {
       private readonly BugContext _context;


        public StatisticsService(BugContext context)
        {
            _context = context;
        }

        public async Task<object> GetDashboardStatsAsync(string role, ClaimsPrincipal user)
        {
            var userId = user.FindFirst("MyApp_Id")?.Value;
            var bugIdsEverFixed = await _context.BugLogs
                .Where(log => log.Action == "StatusChanged : Fixed")
                .Select(log => log.BugId)
                .Distinct()
                .ToListAsync();


            switch (role.ToUpper())
            {
                case "ADMIN":
                    return new
                    {
                        TotalUsers = await _context.Users.CountAsync(),
                        TotalAdmins = await _context.Admins.CountAsync(),
                        TotalDevelopers = await _context.Developers.CountAsync(),
                        TotalActiveDevelopers = await _context.Developers.CountAsync(d => !d.IsDeleted),
                        TotalTesters = await _context.Testers.CountAsync(),
                        TotalActiveTesters = await _context.Testers.CountAsync(t => !t.IsDeleted),
                        TotalBugs = await _context.Bugs.CountAsync(),
                        TotalBugsFixed = await _context.Bugs.CountAsync(b => b.Status == BugStatus.Fixed || b.Status == BugStatus.Closed || b.Status == BugStatus.Verified),
                        TotalBugsInProgress = await _context.Bugs.CountAsync(b => b.Status == BugStatus.InProgress),
                        TotalBugsClosed = await _context.Bugs.CountAsync(b => b.Status == BugStatus.Closed),
                        TotalComments = await _context.Comments.CountAsync(c => c.UserId == userId)
                    };

                case "DEVELOPER":
                    return new
                    {
                        BugsAssigned = await _context.Bugs.CountAsync(b => b.AssignedTo == userId),
                        //BugsFixed = await _context.Bugs.CountAsync(b => b.AssignedTo == userId && (b.Status == BugStatus.Fixed || b.Status == BugStatus.Closed)),
                        bugsFixedByUser = await _context.Bugs
                                                    .Where(b => b.AssignedTo == userId && bugIdsEverFixed.Contains(b.Id))
                                                    .CountAsync(),
                        CodeFiles = await _context.UploadedFileLogs.CountAsync(f => f.DeveloperId == userId),
                        Comments = await _context.Comments.CountAsync(c => c.UserId == userId)
                    };

                case "TESTER":
                    return new
                    {
                        BugsCreated = await _context.Bugs.CountAsync(b => b.CreatedBy == userId),
                        //BugsReopened = await _context.Bugs.CountAsync(b => b.CreatedBy == userId && b.Status == BugStatus.Reopened),
                        BugsDeleted = await _context.Bugs.CountAsync(b => b.CreatedBy == userId && b.IsDeleted),
                        BugsVerified = await _context.Bugs.CountAsync(b => b.CreatedBy == userId && b.Status == BugStatus.Verified || b.Status == BugStatus.Closed),
                        TesterComments = await _context.Comments.CountAsync(c => c.UserId == userId)
                    };

                default:
                    throw new ArgumentException("Invalid role");
            }
        }

    }
}