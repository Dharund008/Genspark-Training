using Bts.Models;
using Bts.Models.DTO;

namespace Bts.Interfaces
{
    public interface IAdminService
    {
        public Task<Admin> AddAdmin(AdminRequestDTO admin);

        public Task<Developer> AddDeveloper(DeveloperRequestDTO developer);

        public Task<Tester> AddTester(TesterRequestDTO tester);

        //bool VerifyPassword(string input, string hashedPassword);
        //Task<bool> IsUserExists(string username);
        public Task<bool> IsEmailExists(string email);
        //Task<bool> IsUsernameExists(string username);
        public Task<Admin> GetByEmail(string email);
        public Task<bool> DeleteDeveloperAsync(string devId);
        public Task<bool> DeleteTesterAsync(string testerId);

        public Task<IEnumerable<Developer>> GetAvailableDevelopersAsync();

        Task<bool> AssignBugToDeveloperAsync(int bugId, string developerId);
        Task<bool> CloseBugAsync(int bugId);
        Task<bool> DeleteBugAsync(int bugId);

        Task<IEnumerable<Bug>> GetAllBugsTesterAsync(string testerId); //can get all bugs created by a tester;

        Task<IEnumerable<Bug>> GetAllBugsDeveloperAsync(string developerId); //can get all bugs assigned to an develoepr
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<Bug>> GetAllBugsAsync();

        Task<List<Developer>> GetAllDevelopersWithDeletedAsync();
        Task<List<Tester>> GetAllTestersWithDeletedAsync();
    }
}