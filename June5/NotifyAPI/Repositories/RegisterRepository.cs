using NotifyAPI.Contexts;
using NotifyAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using NotifyAPI.Models.DTO;
using NotifyAPI.Models;

namespace NotifyAPI.Repositories
{
    public class RegisterRepository : Repository<string, Register>
    {
        public RegisterRepository(ManagementContext context) : base(context)
        {

        }

        public override async Task<Register> Get(string Key)
        {
            var exp = await _context.Registers.SingleOrDefaultAsync(r => r.Username == Key);

            return exp??throw new Exception("No register with the given Username");
        } 
    }
}