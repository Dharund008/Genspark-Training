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
    public class DeveloperRepository : Repository<string, Developer>
    {
        public DeveloperRepository(BugContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Developer>> GetAll()
        {
            var developer = _bugContext.Developers;
            if (developer.Count() == 0)
            {
                throw new Exception("No Developers found");
            }
            return await developer.ToListAsync();
        }
        
        public override async Task<Developer> GetById(string key)
        {
            var developer = await _bugContext.Developers.SingleOrDefaultAsync(d => d.Id == key);
            if (developer == null)
            {
                throw new Exception($"Dev-Repo:No such Developers {key} found");
            }
            return developer;
        }
    }
}