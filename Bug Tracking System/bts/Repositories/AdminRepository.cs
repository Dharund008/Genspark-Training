using Microsoft.EntityFrameworkCore;
using Bts.Interfaces;
using Bts.Models;
using Bts.Models.DTO;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;
using Bts.Contexts;

namespace Bts.Repositories
{
    public class AdminRepository : Repository<string, Admin>
    {
        public AdminRepository(BugContext context) : base(context)
        {
        }

        // public async Task<Admin?> GetAdminByUsername(string username)
        // {
        //     return await _bugContext.Admins.FirstOrDefaultAsync(a => a.Email == username);
        // }
        public override async Task<IEnumerable<Admin>> GetAll()
        {
            var admin = _bugContext.Admins;
            if (admin.Count() == 0)
            {
                throw new Exception("No admins found");
            }
            return await admin.ToListAsync();
        }
        

        
        public override async Task<Admin> GetById(string key)
        {
            var admin = await _bugContext.Admins.SingleOrDefaultAsync(a => a.Id == key);
            if (admin == null)
            {
                throw new Exception($"Ad-Rep:Admin with ID '{key}' not found.");
            }

            return admin;
        }
    }
}