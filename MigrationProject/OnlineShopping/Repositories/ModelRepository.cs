using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;
using Online.Contexts;
using Online.Models;
using Online.Interfaces;

namespace Online.Repositories
{
    public class ModelRepository : Repository<int, Model>
    {
        public ModelRepository(MigrationContext context) : base(context)
        {

        }

        public override async Task<IEnumerable<Model>> GetAllAsync()
        {
            var user = _context.Models;
            if (user.Count() == 0)
            {
                throw new Exception("No Models found");
            }
            return await user.ToListAsync();
        }
        
        public override async Task<Model> GetByIdAsync(int id)
        {
            var user = await _context.Models.FindAsync(id);
            if (user == null)
            {
                throw new Exception("No such Model found");
            }
            return user;
        }
    }
}