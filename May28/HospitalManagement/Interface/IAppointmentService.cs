using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.Models;
using Hospital.Models.DTOs;

namespace Hospital.Interfaces
{
    public interface IAppointmentService
    {
        Task<Appointment> GetAppointmentById(int id);
        Task<ICollection<Appointment>> GetAllAppointments();
        Task<Appointment> AddAppointment(AppointmentAddRequestDto appointment);
        Task<Appointment> UpdateAppointment(int id, AppointmentUpdateRequestDto appointment);
        Task<bool> DeleteAppointment(int id);
    }
}
