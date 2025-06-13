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
    public class BugRepository : Repository<int, Bug>
    {
        public BugRepository(BugContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Bug>> GetAll()
        {
            var bug = _bugContext.Bugs;
            if (bug.Count() == 0)
            {
                throw new Exception("No Bugs found");
            }
            return await bug.ToListAsync();
        }
        
        public override async Task<Bug> GetById(int key)
        {
            var bug = await _bugContext.Bugs.SingleOrDefaultAsync(b => b.Id == key);
            if (bug == null)
            {
                throw new Exception("No such Bug found");
            }
            return bug;
        }
    }
}