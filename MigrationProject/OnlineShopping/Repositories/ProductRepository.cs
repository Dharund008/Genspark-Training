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
    public class ProductRepository : Repository<int, Product>
    {
        public ProductRepository(MigrationContext context) : base(context)
        {

        }
        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }
        
        public override async Task<Product?> GetByIdAsync(int id)
        {
            var user = await _context.Products.FindAsync(id);
            if (user == null)
            {
                throw new Exception("No such Product found");
            }
            return user;
        }
    }
}