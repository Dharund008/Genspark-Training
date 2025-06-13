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
    public class UserRepository : Repository<string, User>
    {
        public UserRepository(BugContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<User>> GetAll()
        {
            var user = _bugContext.Users;
            if (user.Count() == 0)
            {
                throw new Exception("No Users found");
            }
            return await user.ToListAsync();
        }
        
        public override async Task<User> GetById(string name)
        {
            var user = await _bugContext.Users.SingleOrDefaultAsync(u => u.Username == name);
            if (user == null)
            {
                throw new Exception("No such User found");
            }
            return user;
        }
    }
}