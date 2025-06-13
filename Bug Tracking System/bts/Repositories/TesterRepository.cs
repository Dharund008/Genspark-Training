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
    public class TesterRepository : Repository<string, Tester>
    {
        public TesterRepository(BugContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Tester>> GetAll()
        {
            var tester = _bugContext.Testers;
            if (tester.Count() == 0)
            {
                throw new Exception("No Testers found");
            }
            return await tester.ToListAsync();
        }
        
        public override async Task<Tester> GetById(string key)
        {
            var tester = await _bugContext.Testers.SingleOrDefaultAsync(t => t.Id == key);
            if (tester == null)
            {
                throw new Exception("No such Tester found");
            }
            return tester;
        }
    }
}