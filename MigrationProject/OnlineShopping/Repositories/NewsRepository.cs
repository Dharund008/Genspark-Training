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
    public class NewsRepository : Repository<int, News>
    {
        public NewsRepository(MigrationContext context) : base(context)
        {

        }

        public override async Task<IEnumerable<News>> GetAllAsync()
        {
            var user = _context.News;
            if (user.Count() == 0)
            {
                throw new Exception("No News found");
            }
            return await user.ToListAsync();
        }
        
        public override async Task<News> GetByIdAsync(int id)
        {
            var user = await _context.News.FindAsync(id);
            if (user == null)
            {
                throw new Exception("No such News found");
            }
            return user;
        }
    }
}