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
    public class CategoryRepository : Repository<int, Category>
    {
        public CategoryRepository(MigrationContext context) : base(context)
        {

        }

        public override async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }
        
        public override async Task<Category> GetByIdAsync(int id)
        {
            var user = await _context.Categories.FindAsync(id);
            if (user == null)
            {
                throw new Exception("No such Category found");
            }
            return user;
        }
    }
}