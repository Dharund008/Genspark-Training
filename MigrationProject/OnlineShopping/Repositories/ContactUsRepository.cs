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
    public class ContactUsRepository : Repository<int, ContactUs>
    {
        public ContactUsRepository(MigrationContext context) : base(context)
        {

        }

        public override async Task<IEnumerable<ContactUs>> GetAllAsync()
        {
            return await _context.Contacts.ToListAsync();
        }
        
        public override async Task<ContactUs?> GetByIdAsync(int id)
        {
            var user = await _context.Contacts.FindAsync(id);
            if (user == null)
            {
                throw new Exception("No such Contact found");
            }
            return user;
        }
    }
}