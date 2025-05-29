using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hospital.Interfaces;
using Hospital.Models;
using Hospital.Models.DTOs;
using Hospital.Repositories;

namespace Hospital.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly AppointmentRepository _appointmentRepository;

        public AppointmentService(AppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<Appointment> GetAppointmentById(int id)
        {
            var appointment = await _appointmentRepository.Get(id);
            if (appointment == null)
                throw new Exception("Appointment not found");
            return appointment;
        }

        public async Task<ICollection<Appointment>> GetAllAppointments()
        {
            var appointments = await _appointmentRepository.GetAll();
            return appointments.ToList();
        }

        public async Task<Appointment> AddAppointment(AppointmentAddRequestDto appointmentDto)
        {
            if (appointmentDto == null)
                throw new ArgumentNullException(nameof(appointmentDto));

            var appointment = new Appointment
            {
                DoctorId = appointmentDto.DoctorId,
                PatientId = appointmentDto.PatientId,
                AppointmentDate = appointmentDto.AppointmentDate,
                Status = appointmentDto.Status
            };

            await _appointmentRepository.Add(appointment);
            return appointment;
        }

        public async Task<Appointment> UpdateAppointment(int id, AppointmentUpdateRequestDto appointmentDto)
        {
            if (appointmentDto == null)
                throw new ArgumentNullException(nameof(appointmentDto));

            var appointment = await _appointmentRepository.Get(id);
            if (appointment == null)
                throw new Exception("Appointment not found");

            appointment.DoctorId = appointmentDto.DoctorId;
            appointment.PatientId = appointmentDto.PatientId;
            appointment.AppointmentDate = appointmentDto.AppointmentDate;
            appointment.Status = appointmentDto.Status;

            await _appointmentRepository.Update(id, appointment);
            return appointment;
        }

        public async Task<bool> DeleteAppointment(int id)
        {
            var appointment = await _appointmentRepository.Get(id);
            if (appointment == null)
                throw new Exception("Appointment not found");

            await _appointmentRepository.Delete(id);
            return true;
        }
    }
}
