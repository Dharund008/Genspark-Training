using CardiologyAppointmentApp.Models;

namespace CardiologyAppointmentApp.Repositories
{
    public class AppointmentRepository : Repository<int, Appointment>
    {
        private int _idCounter = 100;

        protected override int GenerateID() => _idCounter++;

        public override ICollection<Appointment> GetAll() => _items;

        public override Appointment GetById(int id) =>
            _items.FirstOrDefault(a => a.Id == id);
    }
}
