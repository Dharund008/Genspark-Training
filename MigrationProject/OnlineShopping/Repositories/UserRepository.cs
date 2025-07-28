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
    public class UserRepository : Repository<int, User>
    {
        public UserRepository(Migration context) : base(context)
        {

        }

        public override async Task<IEnumerable<User>> GetAll()
        {
            var user = _context.Users;
            if (user.Count() == 0)
            {
                throw new Exception("No Users found");
            }
            return await user.ToListAsync();
        }
        
        public override async Task<User> GetById(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserId == id);
            if (user == null)
            {
                throw new Exception("No such User found");
            }
            return user;
        }
    }
}