using Bts.Models;
using Bts.Models.DTO;

namespace Bts.Interfaces
{
    public interface IBugService
    {
        public Task<bool> IsBugExists(int id); //search 
        public Task<List<Bug>> GetBugsByStatus(string status); //filter: can check with status

        Task<Bug?> GetBugByIdAsync(int bugId);
        Task<IEnumerable<Bug>> GetAllBugsAsync(int s, int pagesize);
        public Task<List<Bug>> GetBugsByTesterId(string testerId);
        public Task<List<Bug>> GetBugsByDeveloperId(string developerId);

        Task<IEnumerable<Bug>> GetUnassignedBugsAsync();
        public Task<List<Bug>> SearchBugs(string searchTerm);
     }
}