using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Bts.Interfaces;
using Bts.Models;
using Bts.Models.DTO;
using Bts.Repositories;
using Bts.Contexts;
using Bts.MISC;
using Microsoft.AspNetCore.Authorization;


namespace Bts.Services
{
    public class AdminService : IAdminService
    {
        private readonly BugContext _context;
        private readonly IMapper _mapper;
        private readonly IRepository<string, Admin> _adminRepository;
        private readonly IRepository<string, Developer> _developerRepository;
        private readonly IRepository<string, User> _userRepository;
        private readonly IRepository<string, Tester> _testerRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly ILogger<AdminService> _logger;

        private readonly IBugLogService _bugLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdminService(
            BugContext context,
            IMapper mapper,
            IRepository<string, Admin> adminRepository,
            IRepository<string, Developer> developerRepository,
            IRepository<string, User> userRepository,
            IRepository<string, Tester> testerRepository,
            ILogger<AdminService> logger,
            IBugLogService bugLogService,
            IEncryptionService encryptionService,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _context = context;
            _mapper = mapper;
            _adminRepository = adminRepository;
            _developerRepository = developerRepository;
            _userRepository = userRepository;
            _testerRepository = testerRepository;
            _bugLogService = bugLogService;
            _encryptionService = encryptionService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        

        public async Task<Admin> GetByEmail(string mail)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == mail);
            if (admin == null)
            {
                _logger.LogCritical("Admin not found - GetByEmail");
                throw new Exception("Admin not found");
            }
            return admin;
        }

        public async Task<bool> IsEmailExists(string mail)
        {
            // var admin = await _context.Admins.FindAsync(mail); //findasync can be used only for primary keys
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == mail);

            if (admin == null)
            {
                _logger.LogCritical("Admin not found - IsEmailExists");
                return false;
            }
            return true;
        }

        public async Task<Admin> AddAdmin(AdminRequestDTO adreq)
        {
            try
            {
                var emailExists = await IsEmailExists(adreq.Email);
                if (emailExists)
                {
                    throw new Exception("Admin with this email already exists.");
                }
                //var user = _mapper.Map<AdminRequestDTO, User>(adreq);
                var data = new EncryptModel { Data = adreq.Password };
                var encryptedData = await _encryptionService.EncryptData(data);

                var id = await _encryptionService.GenerateId("ADM");

                var user = new User
                {
                    Id = id,
                    Username = adreq.Email,
                    Password = encryptedData.EncryptedString,
                    Role = "ADMIN"
                };
                user = await _userRepository.Add(user);
                Console.WriteLine($"New admin-user Registered : {user.Username} ");
                if (user == null)
                {
                    throw new Exception("Failed to add Admin");
                }

                var admin = new Admin
                {
                    Id = id,
                    Name = adreq.Name,
                    Email = adreq.Email,
                    Password = encryptedData.EncryptedString
                };
                admin = await _adminRepository.Add(admin);
                Console.WriteLine($"New admin Registered : {admin.Name} ");
                if (admin == null)
                {
                    throw new Exception("Failed to add Admin");
                }

                return admin;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Admin services - Add: {ex.Message}");
                throw new Exception($"Failed to add Admin {ex.Message}");
            }
        }

        public async Task<Developer> AddDeveloper(DeveloperRequestDTO devreq)
        {
            try
            {
                var existingDeveloper = await _context.Developers.FirstOrDefaultAsync(d => d.Email == devreq.Email);
                if (existingDeveloper != null)
                {
                    throw new Exception("Developer with this email already exists.");
                }

                var data = new EncryptModel { Data = devreq.Password };
                var encryptedData = await _encryptionService.EncryptData(data);

                var id = await _encryptionService.GenerateId("DEV");

                var user = _mapper.Map<DeveloperRequestDTO, User>(devreq);
                user.Id = id;
                user.Username = devreq.Email;
                user.Role = "DEVELOPER";
                user.Password = encryptedData.EncryptedString;

                user = await _userRepository.Add(user);
                Console.WriteLine($"New developer-user Registered : {user.Username} ");
                if (user == null)
                {
                    throw new Exception("Failed to add Developer");
                }
                var dev = new Developer
                {
                    Id = id,
                    Name = devreq.Name,
                    Email = devreq.Email,
                    Password = encryptedData.EncryptedString
                };
                dev = await _developerRepository.Add(dev);
                Console.WriteLine($"New developer Registered : {dev.Name} ");
                if (dev == null)
                {
                    throw new Exception("Failed to add Developer");
                }
                return dev;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Admin services - Add(developer): {ex.Message}");
                throw new Exception($"Failed to add Developer {ex.Message}");
            }
        }

        //add-tester
        public async Task<Tester> AddTester(TesterRequestDTO tesreq)
        {
            try
            {
                var existingDeveloper = await _context.Testers.FirstOrDefaultAsync(d => d.Email == tesreq.Email);
                if (existingDeveloper != null)
                {
                    throw new Exception("Tester with this email already exists.");
                }

                var data = new EncryptModel { Data = tesreq.Password };
                var encryptedData = await _encryptionService.EncryptData(data);

                var id = await _encryptionService.GenerateId("TES");

                var user = _mapper.Map<TesterRequestDTO, User>(tesreq);
                user.Id = id;
                user.Username = tesreq.Email;
                user.Role = "TESTER";
                user.Password = encryptedData.EncryptedString;

                user = await _userRepository.Add(user);
                Console.WriteLine($"New developer-user Registered : {user.Username} ");
                if (user == null)
                {
                    throw new Exception("Failed to add Developer");
                }
                var tes = new Tester
                {
                    Id = id,
                    Name = tesreq.Name,
                    Email = tesreq.Email,
                    Password = encryptedData.EncryptedString
                };
                tes = await _testerRepository.Add(tes);
                Console.WriteLine($"New developer Registered : {tes.Name} ");
                if (tes == null)
                {
                    throw new Exception("Failed to add Tester");
                }
                return tes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Admin services - Add(tester): {ex.Message}");
                throw new Exception($"Failed to add tester {ex.Message}");
            }
        }

        public async Task<bool> AssignBugToDeveloperAsync(int bugId, string developerId)
        {
            var bug = await _context.Bugs.FindAsync(bugId);
            if (bug == null) return false;
            var isBusy = await _context.Bugs
                .AnyAsync(b => b.AssignedTo == developerId && 
                   b.Status != BugStatus.Closed &&
                   !b.IsDeleted);

            if (isBusy)
                return false;

            bug.AssignedTo = developerId;
            bug.Status = BugStatus.Assigned;
            bug.UpdatedAt = DateTime.UtcNow;

            _context.Bugs.Update(bug);
            var admin = _httpContextAccessor.HttpContext?.User?.FindFirst("MyApp_Id")?.Value; //in services u have to use httpaccessor but not in case of controller
                                                                                              //tobuglog
            
            await _bugLogService.LogEventAsync(bugId, "StatusChanged : Assigned", admin);

            return true;
        }

        public async Task<IEnumerable<Developer>> GetAvailableDevelopersAsync()
        {
            var activeBugStatuses = new[] { BugStatus.New, BugStatus.Assigned, BugStatus.InProgress, BugStatus.Retesting, BugStatus.Reopened };

            var busyDeveloperIds = await _context.Bugs
                .Where(b => !b.IsDeleted && b.AssignedTo != null && activeBugStatuses.Contains(b.Status))
                .Select(b => b.AssignedTo)
                .Distinct()
                .ToListAsync();

            var availableDevelopers = await _context.Developers
                .Where(d => !busyDeveloperIds.Contains(d.Id) && !d.IsDeleted)
                .ToListAsync();

            return availableDevelopers;
        }


        public async Task<bool> CloseBugAsync(int bugId)
        {
            var bug = await _context.Bugs.FindAsync(bugId);
            if (bug == null) return false;

            bug.Status = BugStatus.Closed;
            bug.UpdatedAt = DateTime.UtcNow;

            _context.Bugs.Update(bug);
            await _context.SaveChangesAsync();
            var admin = _httpContextAccessor.HttpContext?.User?.FindFirst("MyApp_Id")?.Value;
            //tobuglog
            await _bugLogService.LogEventAsync(bugId, "StatusChanged : Closed", admin);

            return true;
        }
        public async Task<bool> DeleteBugAsync(int bugId)
        {
            var bug = await _context.Bugs.FindAsync(bugId);
            if (bug == null) return false;

            bug.IsDeleted = true;
            _context.Bugs.Update(bug);
            await _context.SaveChangesAsync();
            var admin = _httpContextAccessor.HttpContext?.User?.FindFirst("MyApp_Id")?.Value;
            //tobuglog
            await _bugLogService.LogEventAsync(bugId, "Changed-Isdeleted : true", admin);
            return true;
        }

        public async Task<IEnumerable<Bug>> GetAllBugsTesterAsync(string testerId)
        {
            var bugs = await _context.Bugs
                    .Include(b => b.CreatedByTester)
                    .Where(b => b.CreatedBy == testerId)
                    .ToListAsync();
            return bugs;
        }

        public async Task<IEnumerable<Bug>> GetAllBugsDeveloperAsync(string developerId)
        {
            var bugs = await _context.Bugs
                    .Include(b => b.AssignedToDeveloper)
                    .Where(b => b.AssignedTo == developerId)
                    .ToListAsync();
            return bugs;
        }
        
        public async Task<bool> DeleteDeveloperAsync(string devId)
        {
            var dev = await _context.Developers.FindAsync(devId);
            if (dev == null) return false;

            else dev.IsDeleted = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTesterAsync(string testerId)
        {
            var tester = await _context.Testers.FindAsync(testerId);
            if (tester == null) return false;

            else tester.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<Bug>> GetAllBugsAsync()
        {
            return await _context.Bugs.ToListAsync();
        }
        
    }
}