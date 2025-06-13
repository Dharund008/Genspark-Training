using Bts.Models;
using Bts.Models.DTO;

namespace Bts.Interfaces
{
    public interface ITesterService
    {

        //Task<Tester> UpdateTester(int id);
        Task<Bug> CreateBugAsync(BugSubmissionDTO dto);

        Task<Bug> UpdateBugAsync(int bugId,BugSubmissionDTO dto);

        Task<IEnumerable<Bug>> GetMyReportedBugsAsync(string testerId);
        Task<bool> UpdateBugStatusAsync(int bugId, BugStatus newStatus);
        // Task<Comment> AddCommentAsync(Comment comment);
        Task<Tester> GetTesterByEmail(string email);

        // Task<Tester> GetTesterById(int id);
        Task<List<Tester>> GetAllTesters();
        Task<List<Tester>> GetTesterWithBugs();//tester id

        Task<List<Tester>> GetTestersAssociatedWithBugs();
    }
}