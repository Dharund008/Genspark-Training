using Hospital.Contexts;
using Hospital.Interfaces;
using Hospital.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Repositories
{
    public  class DoctorSpecialityRepository : Repository<int, DoctorSpeciality>
    {
        protected DoctorSpecialityRepository(ClinicContext clinicContext) : base(clinicContext)
        {
        }

        public override async Task<DoctorSpeciality> Get(int key)
        {
            var doctorSpecialities = await _clinicContext.DoctorSpecialities.SingleOrDefaultAsync(p => p.SerialNumber == key);

            return doctorSpecialities??throw new Exception("No doctor specialities with the given ID");
        }

        public override async Task<IEnumerable<DoctorSpeciality>> GetAll()
        {
            var doctorSpeciality = _clinicContext.DoctorSpecialities;
            if (doctorSpeciality.Count() == 0)
                throw new Exception("No doctor speciality in the database");
            return (await doctorSpeciality.ToListAsync());
        }
    }
}