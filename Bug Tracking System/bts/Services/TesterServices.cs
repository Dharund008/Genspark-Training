using Bts.Interfaces;
using Bts.Models;
using Bts.Contexts;
using Bts.Models.DTO;
using Bts.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;
namespace Bts.Services
{
    public class TesterService : ITesterService
    {
        private readonly BugContext _context;
        private readonly ICurrentUserService _currentUserService;

        private readonly IRepository<string, User> _userRepository;
         private readonly IBugLogService _bugLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<TesterService> _logger;

        public TesterService(BugContext context, ICurrentUserService currentUserService, IRepository<string, User> userRepository, IBugLogService bugLogService,
                                IHttpContextAccessor httpContextAccessor,
                                ILogger<TesterService> logger)
        {
            _context = context;
            _currentUserService = currentUserService;
            _userRepository = userRepository;
            _bugLogService = bugLogService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<Tester> GetTesterByEmail(string email)
        {
            var tester = await _context.Testers.FirstOrDefaultAsync(a => a.Email == email);
            if (tester == null)
            {
                _logger.LogWarning("Tester with email {Email} not found in GetTesterByEmail", email);
                throw new Exception($"Tester with email '{email}' not found.");
            }
            _logger.LogInformation("Retrieved tester by email {Email}", email);
            return tester;
        }

        public async Task<Bug> CreateBugAsync(BugSubmissionDTO dto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == _currentUserService.Id);
            var tester = await _context.Testers.FirstOrDefaultAsync(t => t.Id == _currentUserService.Id);
            if (tester == null)
            {
                _logger.LogWarning("Tester not found with ID {TesterId} in CreateBugAsync", _currentUserService.Id);
                throw new Exception("Tester not found");
            }
            if (user.Id != tester.Id)
            {
                _logger.LogWarning("User {UserId} is not authorized to create bugs in CreateBugAsync", user.Id);
                throw new Exception("User is not authorized to create bugs.");
            }
            var bug = new Bug
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                ScreenshotUrl = dto.ScreenshotUrl,
                CreatedBy = tester.Id,
                Status = BugStatus.New,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Bugs.Add(bug);
            await _context.SaveChangesAsync();
            //buglog
            //tobuglog
            await _bugLogService.LogEventAsync(bug.Id, "Bug Created : New", _currentUserService.Id);
            _logger.LogInformation("Created new bug {BugId} by tester {TesterId}", bug.Id, _currentUserService.Id);
            return bug;
        }

        // public async Task<Bug> UpdateBugAsync(int bugId, BugSubmissionDTO dto)
        // {
        //     _logger.LogInformation("UpdateBugAsync called by tester {TesterId} for bug {BugId}", _currentUserService.Id, bugId);

        //     var tester = await _context.Testers.FirstOrDefaultAsync(t => t.Id == _currentUserService.Id);
        //     if (tester == null)
        //     {
        //         _logger.LogWarning("Tester not found with ID {TesterId} in UpdateBugAsync", _currentUserService.Id);
        //         throw new Exception("Tester not found");
        //     }

        //     var bug = await _context.Bugs.FirstOrDefaultAsync(b => b.Id == bugId && b.CreatedBy == tester.Id);
        //     if (bug == null)
        //     {
        //         _logger.LogWarning("Bug {BugId} not found or unauthorized for tester {TesterId} in UpdateBugAsync", bugId, tester.Id);
        //         throw new Exception("Bug not found or Unauthorized");
        //     }

        //     // Log before updating fields
        //     _logger.LogDebug("Updating bug fields for bug {BugId}", bugId);

        //     // Update only fields that are provided (partial update behavior)
        //     if (!string.IsNullOrEmpty(dto.Title)) bug.Title = dto.Title;
        //     if (!string.IsNullOrEmpty(dto.Description)) bug.Description = dto.Description;
        //     if (dto.Priority != null) bug.Priority = dto.Priority;
        //     if (!string.IsNullOrEmpty(dto.ScreenshotUrl)) bug.ScreenshotUrl = dto.ScreenshotUrl;

        //     bug.UpdatedAt = DateTime.UtcNow;
        //     _context.Bugs.Update(bug);

        //     await _context.SaveChangesAsync();
        //     _logger.LogInformation("Bug {BugId} updated in database by tester {TesterId}", bugId, tester.Id);

        //     // Log bug event
        //     await _bugLogService.LogEventAsync(bugId, "Bug details Updated", _currentUserService.Id);
        //     _logger.LogInformation("Bug update event logged for bug {BugId} by tester {TesterId}", bugId, _currentUserService.Id);

        //     return bug;
        // }

        public async Task<Bug> UpdateBugAsync(int bugId, UpdateBugPatchDTO dto)
        {
            _logger.LogInformation("UpdatePatchBugAsync called by tester {TesterId} for bug {BugId}", _currentUserService.Id, bugId);

            var tester = await _context.Testers.FirstOrDefaultAsync(t => t.Id == _currentUserService.Id);
            if (tester == null)
            {
                _logger.LogWarning("Tester not found with ID {TesterId} in UpdateBugAsync", _currentUserService.Id);
                throw new Exception("Tester not found");
            }

            var bug = await _context.Bugs.FirstOrDefaultAsync(b => b.Id == bugId && b.CreatedBy == tester.Id);
            if (bug == null)
            {
                _logger.LogWarning("Bug {BugId} not found or unauthorized for tester {TesterId} in UpdateBugAsync", bugId, tester.Id);
                throw new Exception("Bug not found or Unauthorized");
            }

            // Log before updating fields
            _logger.LogDebug("Updating bug fields for bug {BugId}", bugId);

            // Update only fields that are provided (partial update behavior)
            if (dto.Description != null) bug.Description = dto.Description;
            if (dto.Priority != null) bug.Priority = dto.Priority;
            if (dto.ScreenshotUrl != null) bug.ScreenshotUrl = dto.ScreenshotUrl;

            bug.UpdatedAt = DateTime.UtcNow;
            _context.Bugs.Update(bug);

            await _context.SaveChangesAsync();
            _logger.LogInformation("Bug {BugId} updated in database by tester {TesterId}", bugId, tester.Id);

            // Log bug event
            await _bugLogService.LogEventAsync(bugId, "Bug details Updated", _currentUserService.Id);
            _logger.LogInformation("Bug update event logged for bug {BugId} by tester {TesterId}", bugId, _currentUserService.Id);

            return bug;
        }

        //search : gets bugs created by respective user
        public async Task<IEnumerable<Bug>> GetMyReportedBugsAsync(string testerId)
        {
            var bugs = await _context.Bugs
                .Where(b => b.CreatedBy == testerId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
            _logger.LogInformation("Retrieved reported bugs for tester {TesterId}", testerId);
            return bugs;
        }

        public async Task<bool> UpdateBugStatusAsync(int bugId, BugStatus newStatus)
        {
            var bug = await _context.Bugs.FindAsync(bugId);
            if (bug == null)
            {
                _logger.LogWarning("Bug {BugId} not found in UpdateBugStatusAsync", bugId);
                return false;
            }

            // Only allow Tester-appropriate statuses
            if (newStatus == BugStatus.Verified ||
                newStatus == BugStatus.Retesting ||
                newStatus == BugStatus.Reopened)
            {
                bug.Status = newStatus;
                bug.UpdatedAt = DateTime.UtcNow;

                _context.Bugs.Update(bug);
                await _context.SaveChangesAsync();
                //buglog
                await _bugLogService.LogEventAsync(bugId, $"Bug Status changed {newStatus}", _currentUserService.Id);
                _logger.LogInformation("Updated bug status {NewStatus} for bug {BugId}", newStatus, bugId);
                return true;
            }

            return false;
        }

        // public async Task<Comment> AddCommentAsync(Comment comment)
        // {
        //     comment.CreatedAt = DateTime.UtcNow;
        //     _context.Comments.Add(comment);
        //     await _context.SaveChangesAsync();
        //     return comment;
        // }
        
        public async Task<List<Tester>> GetAllTesters()
        {
            var testers = await _context.Testers.ToListAsync();
            _logger.LogInformation("Retrieved all testers");
            return testers;
        }

        public async Task<List<Tester>> GetTesterWithBugs()
        {
            var test = await _userRepository.GetById(_currentUserService.Id);
            // Get the tester and include their reported bugs
            var tester = await _context.Testers
                .Include(t => t.Bugs)
                .FirstOrDefaultAsync(t => t.Id == test.Id);

            if (tester == null)
            {
                _logger.LogWarning("Tester with ID {TesterId} not found in GetTesterWithBugs", _currentUserService.Id);
                return new List<Tester>();
            }

            _logger.LogInformation("Retrieved tester with bugs for tester ID {TesterId}", _currentUserService.Id);
            return new List<Tester> { tester };
        }

        public async Task<List<Tester>> GetTestersAssociatedWithBugs()
        {
            // Get testers who have reported at least one bug
            var testers = await _context.Testers
                .Where(t => t.Bugs.Any())
                .Include(t => t.Bugs)
                .ToListAsync();
            _logger.LogInformation("Retrieved testers associated with bugs");
            return testers;
        }
    }
}
