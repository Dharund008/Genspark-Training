using Microsoft.EntityFrameworkCore;
using Bts.Interfaces;
using Bts.Models;
using Bts.Models.DTO;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;
using Bts.Contexts;

namespace Bts.Repositories
{
    public class CommentRepository : Repository<int, Comment>
    {
        public CommentRepository(BugContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Comment>> GetAll()
        {
            var comment = _bugContext.Comments;
            if (comment.Count() == 0)
            {
                throw new Exception("No Comments found");
            }
            return await comment.ToListAsync();
        }
        
        public override async Task<Comment> GetById(int key)
        {
            var comment = await _bugContext.Comments.SingleOrDefaultAsync(c => c.Id == key);
            if (comment == null)
            {
                throw new Exception($"Comment with ID {key} not found.");
            }
            return comment;
        }
    }

}