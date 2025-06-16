using Bts.Models;
using Bts.Models.DTO;

namespace Bts.Interfaces
{
    public interface IDeveloperService
    {
        Task<Developer> GetDeveloperByEmail(string email);

        Task<List<Developer>> GetAllDevelopers();
        Task<List<Developer>> GetDeveloperWithBugs();

        Task<List<Bug>> GetAssignedBugsAsync(string developerId);
        Task<bool> UpdateBugStatusAsync(int bugId, BugStatus newStatus);

        Task<string> UploadCodeAsync(IFormFile file, string developerId);

        Task<IEnumerable<TesterInfoDTO>> GetTestersForMyBugsAsync(string developerId);
        // Task<Comment> AddCommentAsync(Comment comment);
    }
}