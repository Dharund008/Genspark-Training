using NotifyAPI.Contexts;
using NotifyAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using NotifyAPI.Models.DTO;
using NotifyAPI.Models;

namespace NotifyAPI.Repositories
{
    public class UserRepository : Repository<string, User>
    {
        public UserRepository(ManagementContext context) : base(context)
        {

        }

        public override async Task<User> Get(string Key)
        {
            var exp = await _context.Users.SingleOrDefaultAsync(u => u.Name == Key);

            return exp??throw new Exception("No User with the given Name");
        } 
    }
}