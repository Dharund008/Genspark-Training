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
    public class ColorRepository : Repository<int, Color>
    {
        public ColorRepository(MigrationContext context) : base(context)
        {

        }

        public override async Task<IEnumerable<Color>> GetAllAsync()
        {
            var user = _context.Colors;
            if (user.Count() == 0)
            {
                throw new Exception("No Colors found");
            }
            return await user.ToListAsync();
        }
        
        public override async Task<Color> GetByIdAsync(int id)
        {
            var user = await _context.Colors.FindAsync(id);
            if (user == null)
            {
                throw new Exception("No such Color found");
            }
            return user;
        }
    }
}