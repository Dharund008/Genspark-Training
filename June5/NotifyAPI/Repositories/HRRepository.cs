using NotifyAPI.Contexts;
using NotifyAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using NotifyAPI.Models.DTO;
using NotifyAPI.Models;

namespace NotifyAPI.Repositories
{
    public class HRRepository : Repository<string, HRAdmin>
    {
        public HRRepository(ManagementContext context) : base(context)
        {

        }

        public override async Task<HRAdmin> Get(string Key)
        {
            var exp = await _context.HRAdmins.SingleOrDefaultAsync(h => h.Name == Key);

            return exp??throw new Exception("No HR with the given Name");
        } 
    }
}