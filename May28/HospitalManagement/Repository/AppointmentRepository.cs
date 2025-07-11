using Hospital.Contexts;
using Hospital.Interfaces;
using Hospital.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Repositories
{
    public  class AppointmnetRepository : Repository<string, Appointmnet>
    {
        protected AppointmnetRepository(ClinicContext clinicContext) : base(clinicContext)
        {
        }

        public override async Task<Appointmnet> Get(string key)
        {
            var appointment = await _clinicContext.Appointmnets.SingleOrDefaultAsync(p => p.AppointmnetNumber == key);

            return appointment??throw new Exception("No appointmnet with the given ID");
        }

        public override async Task<IEnumerable<Appointmnet>> GetAll()
        {
            var appointments = _clinicContext.Appointmnets;
            if (appointments.Count() == 0)
                throw new Exception("No Appointment in the database");
            return (await appointments.ToListAsync());
        }
    }
}