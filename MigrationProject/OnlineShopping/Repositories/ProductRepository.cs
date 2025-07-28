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
            var user = _context.Products;
            if (user.Count() == 0)
            {
                throw new Exception("No Products found");
            }
            return await user.ToListAsync();
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