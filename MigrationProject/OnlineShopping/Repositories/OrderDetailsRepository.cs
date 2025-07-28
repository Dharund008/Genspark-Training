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
    public class OrderDetailsRepository : Repository<int, OrderDetail>
    {
        public OrderDetailsRepository(MigrationContext context) : base(context)
        {

        }
        public override async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            var user = _context.OrderDetails;
            if (user.Count() == 0)
            {
                throw new Exception("No OrderDetails found");
            }
            return await user.ToListAsync();
        }
        
        public override async Task<OrderDetail> GetByIdAsync(int id)
        {
            var user = await _context.OrderDetails.FindAsync(id);
            if (user == null)
            {
                throw new Exception("No such OrderDetail found");
            }
            return user;
        }
    }
}