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
    public class OrderRepository : Repository<int, Order>
    {
        public OrderRepository(MigrationContext context) : base(context)
        {

        }

        public override async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }
        
        public override async Task<Order?> GetByIdAsync(int id)
        {
            var user = await _context.Orders.FindAsync(id);
            if (user == null)
            {
                throw new Exception("No such Order found");
            }
            return user;
        }
    }

}