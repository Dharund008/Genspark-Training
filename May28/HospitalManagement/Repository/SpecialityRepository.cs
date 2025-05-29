using Hospital.Contexts;
using Hospital.Interfaces;
using Hospital.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Repositories
{
    public  class SpecialityRepository : Repository<int, Speciality>
    {
        protected SpecialityRepository(ClinicContext clinicContext) : base(clinicContext)
        {
        }

        public override async Task<Speciality> Get(int key)
        {
            var speciality = await _clinicContext.Specialities.SingleOrDefaultAsync(p => p.Id == key);

            return speciality??throw new Exception("No speciality with teh given ID");
        }

        public override async Task<IEnumerable<Speciality>> GetAll()
        {
            var specialities = _clinicContext.Specialities;
            if (specialities.Count() == 0)
                throw new Exception("No Speciality in the database");
            return (await specialities.ToListAsync());
        }
    }
}