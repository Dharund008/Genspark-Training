using CardiologyAppointmentApp.Models;
using CardiologyAppointmentApp.Interfaces;

namespace CardiologyAppointmentApp.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepositor<int, Appointment> _repo;

        public AppointmentService(IRepositor<int, Appointment> repo)
        {
            _repo = repo;
        }

        public int AddAppointment(Appointment appointment)
        {
            try
            {
                var added = _repo.Add(appointment);
                return added?.Id ?? -1;
            }
            catch (Exception e)
            {
                Console.WriteLine("Add Error: " + e.Message);
                return -1;
            }
        }

        public List<Appointment>? SearchAppointments(AppointmentSearchModel model)
        {
            try
            {
                var data = _repo.GetAll();
                data = FilterByName(data, model.PatientName);
                data = FilterByDate(data, model.AppointmentDate);
                data = FilterByAge(data, model.AgeRange);
                return data.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine("Search Error: " + e.Message);
                return null;
            }
        }

        private ICollection<Appointment> FilterByName(ICollection<Appointment> apps, string? name)
        {
            return string.IsNullOrEmpty(name) ? apps :
                apps.Where(a => a.PatientName.ToLower().Contains(name.ToLower())).ToList();
        }

        private ICollection<Appointment> FilterByDate(ICollection<Appointment> apps, DateTime? date)
        {
            return date == null ? apps :
                apps.Where(a => a.AppointmentDate.Date == date.Value.Date).ToList();
        }

        private ICollection<Appointment> FilterByAge(ICollection<Appointment> apps, Range<int>? range)
        {
            return range == null ? apps :
                apps.Where(a => a.PatientAge >= range.MinVal && a.PatientAge <= range.MaxVal).ToList();
        }
    }
}
