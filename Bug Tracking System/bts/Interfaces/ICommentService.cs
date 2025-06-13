using Bts.Models;
using Bts.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bts.Interfaces
{
    public interface ICommentService
    {
        Task<Comment> AddCommentAsync(CommentRequestDTO dto, string userId);
        Task<IEnumerable<Comment>> GetCommentsByBugIdAsync(int bugId);

        Task<List<Comment>> GetAllComments();

        Task<List<Comment>> GetCommentsByUserRole(string userRole); // admin/tester/developer


    }
}
