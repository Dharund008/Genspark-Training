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
    public class CartRepository : Repository<int, Cart>
    {
        public CartRepository(MigrationContext context) : base(context)
        {

        }

        public override async Task<IEnumerable<Cart>> GetAllAsync()
        {
            var user = _context.Carts;
            if (user.Count() == 0)
            {
                throw new Exception("No Carts found");
            }
            return await user.ToListAsync();
        }
        
        public override async Task<Cart> GetByIdAsync(int id)
        {
            var user = await _context.Carts.SingleOrDefaultAsync(u => u.CartId == id);
            if (user == null)
            {
                throw new Exception("No such Cart found");
            }
            return user;
        }
    }
}