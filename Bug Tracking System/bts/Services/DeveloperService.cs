using Bts.Interfaces;
using Bts.Models;
using Bts.Contexts;
using Bts.Models.DTO;
using Bts.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bts.Hubs;
using Microsoft.AspNetCore.SignalR;

using Microsoft.Extensions.Logging;
namespace Bts.Services
{
    public class DeveloperService : IDeveloperService
    {
        private readonly BugContext _context;

        private readonly ICurrentUserService _currentUserService;

        private readonly IRepository<string, Developer> _developerRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
         private readonly IBugLogService _bugLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<DeveloperService> _logger;



        public DeveloperService(BugContext context, ICurrentUserService currentUserService, IRepository<string, Developer> developerRepository,
                                IHubContext<NotificationHub> hub,
                                IBugLogService bugLogService,
                                IHttpContextAccessor httpContextAccessor,
                                ILogger<DeveloperService> logger)
        {
            _context = context;
            _currentUserService = currentUserService;
            _developerRepository = developerRepository;
            _hubContext = hub;
            _bugLogService = bugLogService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<Developer> GetDeveloperByEmail(string email)
        {
            var developer = await _context.Developers.FirstOrDefaultAsync(a => a.Email == email);
            if (developer == null)
            {
                _logger.LogWarning("Developer with email {Email} not found in GetDeveloperByEmail", email);
                throw new Exception($"Developer with email '{email}' not found.");
            }
            _logger.LogInformation("Retrieved developer by email {Email}", email);
            return developer;
        }

        public async Task<List<Bug>> GetAssignedBugsAsync(string developerId)
        {
            var bugs = await _context.Bugs
                .Where(b => b.AssignedTo == developerId && !b.IsDeleted)
                .ToListAsync();
            _logger.LogInformation("Retrieved assigned bugs for developer {DeveloperId}", developerId);
            return bugs;
        }

        public async Task<bool> UpdateBugStatusAsync(int bugId, BugStatus newStatus)
        {
            var developer = await _developerRepository.GetById(_currentUserService.Id);
            var bug = await _context.Bugs.FindAsync(bugId);
            if (bug == null || bug.AssignedTo != developer.Id)
            {
                _logger.LogWarning("Bug {BugId} not found or not assigned to developer {DeveloperId} in UpdateBugStatusAsync", bugId, _currentUserService.Id);
                return false;
            }

            // Allow only developer-valid statuses
            var allowedStatuses = new[]
            {
                BugStatus.InProgress,
                BugStatus.Fixed
            };

            if (!allowedStatuses.Contains(newStatus))
            {
                _logger.LogWarning("Invalid status {NewStatus} in UpdateBugStatusAsync for bug {BugId}", newStatus, bugId);
                return false;
            }

            bug.Status = newStatus;
            bug.UpdatedAt = DateTime.UtcNow;

            _context.Bugs.Update(bug);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.Group("ADMIN")
                    .SendAsync("ReceiveMessage", $"{_currentUserService.Id} has updated the {bugId} to {newStatus}");
            await _hubContext.Clients.Group("TESTER")
                    .SendAsync("ReceiveMessage", $"{_currentUserService.Id} has updated the {bugId} to {newStatus}");

            var dev = _httpContextAccessor.HttpContext?.User?.FindFirst("MyApp_Id")?.Value;
            //tobuglog
            await _bugLogService.LogEventAsync(bugId, $"StatusChanged : {newStatus}", dev);

            return true;
        }

        // public async Task<Comment> AddCommentAsync(Comment comment)
        // {
        //     comment.CreatedAt = DateTime.UtcNow;
        //     _context.Comments.Add(comment);
        //     await _context.SaveChangesAsync();
        //     return comment;
        // }

        public async Task<List<Developer>> GetAllDevelopers()
        {
            var developers = await _context.Developers.ToListAsync();
            _logger.LogInformation("Retrieved all developers");
            return developers;
        }

        public async Task<List<Developer>> GetDeveloperWithBugs()
        {
            var developer = await _developerRepository.GetById(_currentUserService.Id);

            var dev = await _context.Developers
                .Include(t => t.Bugs)
                .FirstOrDefaultAsync(t => t.Id == developer.Id);

            if (dev == null)
            {
                _logger.LogWarning("Developer with ID {DeveloperId} not found in GetDeveloperWithBugs", _currentUserService.Id);
                return new List<Developer>();
            }

            _logger.LogInformation("Retrieved developer with bugs for developer ID {DeveloperId}", _currentUserService.Id);
            return new List<Developer> { dev };
        }
        
        public async Task<string> UploadCodeAsync(IFormFile file, string developerId)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("No file uploaded in UploadCodeAsync for developer {DeveloperId}", developerId);
                throw new Exception("No file uploaded.");
            }

            var folderPath = Path.Combine("wwwroot", "uploads", "dev_code");
            Directory.CreateDirectory(folderPath); // ensure exists

            var fileName = $"{developerId}_{DateTime.UtcNow.Ticks}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var relativePath = $"/uploads/dev_code/{fileName}";

            // Save to DB
            var log = new UploadedFileLog
            {
                DeveloperId = developerId,
                FileName = fileName,
                FilePath = relativePath,
                UploadedAt = DateTime.UtcNow
            };

            _context.UploadedFileLogs.Add(log);
            await _context.SaveChangesAsync();

            // // Notify testers & admin
            // await _hubContext.Clients.Group("TESTER")
            //     .SendAsync("ReceiveMessage", $"New code uploaded by {developerId}: {relativePath}");
            // await _hubContext.Clients.Group("ADMIN")
            //     .SendAsync("ReceiveMessage", $"Developer {developerId} uploaded code for review.");

            _logger.LogInformation("Developer {DeveloperId} uploaded code: {FilePath}", developerId, relativePath);
            return relativePath;
        }


    }
}
