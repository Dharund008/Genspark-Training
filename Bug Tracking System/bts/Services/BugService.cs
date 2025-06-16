using Bts.Interfaces;
using Bts.Models;
using Bts.Models.DTO;
using Bts.Repositories;
using Bts.Services;
using Bts.Contexts;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;
namespace Bts.Services
{
    public class BugService : IBugService
    {
        private readonly BugContext _context;
        private readonly IMapper _mapper;
        private readonly IRepository<int, Bug> _bugrepository;
        private readonly IRepository<string, Tester> _testerRepository;

        private readonly IRepository<string, Developer> _developerRepository;
        private readonly ILogger<BugService> _logger;

        public BugService(
            BugContext context,
            IMapper mapper,
            IRepository<int, Bug> bugrepository,
            IRepository<string, Tester> testerRepository,
            IRepository<string, Developer> developerRepository,
            ILogger<BugService> logger
           )
        {
            _context = context;
            _mapper = mapper;
            _bugrepository = bugrepository;
            _testerRepository = testerRepository;
            _developerRepository = developerRepository;
            _logger = logger;
        }


        public async Task<bool> IsBugExists(int id)
        {
            var bug = await _bugrepository.GetById(id);
            if (bug == null)
            {
                _logger.LogWarning("Bug not found with ID {BugId} in IsBugExists", id);
                return false;
            }
            return true;
        }

        public async Task<List<Bug>> GetBugsByStatus(string status)
        {
            if (!Enum.TryParse(typeof(BugStatus), status, true, out var parsedStatus))
            {
                _logger.LogWarning("Invalid bug status {Status} in GetBugsByStatus", status);
                return new List<Bug>();
            }
            var bugs = await _context.Bugs
                .Where(b => b.Status == (BugStatus)parsedStatus)
                .ToListAsync();
            _logger.LogInformation("Retrieved bugs by status {Status}", status);
            return bugs;
        }

        public async Task<Bug?> GetBugByIdAsync(int id)
        {
            var bugExists = await IsBugExists(id);
            if (bugExists)
            {
                var bug = await _context.Bugs
                .Include(b => b.CreatedByTester)
                .Include(b => b.AssignedToDeveloper)
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
                _logger.LogInformation("Retrieved bug by ID {BugId}", id);
                return bug;
            }
            else
            {
                _logger.LogWarning("Bug with ID {BugId} does not exist in GetBugByIdAsync", id);
                throw new Exception($"Bug with ID {id} does not exist.");
            }
        }

        // public async Task<IEnumerable<Bug>> GetAllBugsAsync()
        // {
        //     return await _context.Bugs
        //         .Where(b => !b.IsDeleted)
        //         .ToListAsync();
        // }

        public async Task<IEnumerable<Bug>> GetAllBugsAsync(int page, int pageSize)
        {
            if (page < 1 || pageSize < 1)
            {
                _logger.LogWarning("Invalid page {Page} or pageSize {PageSize} in GetAllBugsAsync", page, pageSize);
                throw new ArgumentException("Page number and page size must be greater than zero.");
            }

            var bugs = await _context.Bugs
                .Where(b => !b.IsDeleted)
                .OrderByDescending(b => b.CreatedAt) // Sort bugs by newest first
                .Skip((page - 1) * pageSize) // Skip previous pages
                .Take(pageSize) // Limit results to the requested page
                .ToListAsync();
            _logger.LogInformation("Retrieved bugs page {Page} with page size {PageSize}", page, pageSize);
            return bugs;
        }
        
        public async Task<IEnumerable<Bug>> GetUnassignedBugsAsync()
        {
            return await _context.Bugs
                .Where(b => string.IsNullOrEmpty(b.AssignedTo))
                .ToListAsync();
        }


        public async Task<List<Bug>> GetBugsByTesterId(string testerId)
        {
            var bugs = await _context.Bugs
                .Include(b => b.CreatedByTester)
                .Where(b => b.CreatedBy == testerId)
                .ToListAsync();
            _logger.LogInformation("Retrieved bugs by tester ID {TesterId}", testerId);
            return bugs;
        }

        public async Task<List<Bug>> GetBugsByDeveloperId(string developerId)
        {
                var bugs = await _context.Bugs
                    .Include(b => b.AssignedToDeveloper)
                    .Where(b => b.AssignedTo == developerId)
                    .ToListAsync();
                _logger.LogInformation("Retrieved bugs by developer ID {DeveloperId}", developerId);
                return bugs;
        }

       

        public async Task<List<Bug>> SearchBugs(string searchTerm)
        {
            var bugs = await _context.Bugs
                .Where(b => b.Title.Contains(searchTerm) || b.Description.Contains(searchTerm))
                .ToListAsync();
            _logger.LogInformation("Searched bugs with term {SearchTerm}", searchTerm);
            return bugs;
        }

    }
}
