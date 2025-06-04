using System.Linq.Expressions;
using FirstAPI.Contexts;
using FirstAPI.Interfaces;
using FirstAPI.Misc;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using Microsoft.EntityFrameworkCore;

namespace FirstAPI.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        private readonly ClinicContext context;
        private readonly IAuthenticationService _authenticationService;
        private readonly IEncryptionService _encryptionService;

        private readonly IRepository<string, Appointmnet> _appointmentRepository;
        private readonly IPatientService _patientServiceForGet;

        public AppointmentService(IPatientService patientService, IDoctorService doctorservice,
                                  ClinicContext clinicContext,
                                  IEncryptionService encryptionService,
                                  IAuthenticationService authenticationService,
                                  IRepository<string, Appointmnet> appointmentRepository,
                                  IPatientService patientServiceForGet)
        {
            _patientService = patientService;
            _doctorService = doctorservice;
            _encryptionService = encryptionService;
            context = clinicContext;
            _authenticationService = authenticationService;
            _appointmentRepository = appointmentRepository;
            _patientServiceForGet = patientServiceForGet;
        }

        public async Task<Appointmnet> AddAppointment(AppointmentRequestDTO appointment)
        {
            var patient = await _patientServiceForGet.GetPatient(appointment.PatientEmail);
            if (patient == null)
            {
                throw new Exception("Patient not found");
            }
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == appointment.PatientEmail);
            if (user == null)
                throw new Exception("Patient Not Authenticated!");

            //Encrypt the provided password using the stored hash key
            var encryptedData = await _encryptionService.EncryptData(new EncryptModel
            {
                Data = appointment.PatientPassword,
                HashKey = user.HashKey
            });

            bool isPasswordValid = true;
            for (int i = 0; i < encryptedData.EncryptedData.Length; i++)
            {
                if (encryptedData.EncryptedData[i] != user.Password[i])
                {
                    isPasswordValid = false;
                    break;
                }
            }
            if (!isPasswordValid)
            {
                throw new Exception("Invalid patient password!");
            }

            var doctor = await _doctorService.GetDoctByName(appointment.DoctorName);
            if (doctor == null)
            {
                throw new Exception("Doctor not found");
            }

            //checking if the appointment already exist for the patient and doctor
            var existingAppointment = doctor.Appointmnets?.Where(a => a.AppointmnetDateTime == appointment.AppointmentTime).ToList();
            if (existingAppointment != null && existingAppointment.Count > 0)
            {
                throw new Exception("Appointment already exists for this time slot");
            }
            else
            {
                var newAppointment = new Appointmnet
                {
                    AppointmnetNumber = GenerateAppointment(),
                    PatientId = patient.Id,
                    DoctorId = doctor.Id,
                    AppointmnetDateTime = appointment.AppointmentTime,
                    Status = "Success"
                };
                newAppointment = await _appointmentRepository.Add(newAppointment);
                if (newAppointment == null)
                {
                    throw new Exception("Failed to add appointment");
                }
                return newAppointment;
            }

        }

        public static string GenerateAppointment()
        {
            var random = new Random();
            var appointmentNumber = "APPT" + random.Next(1000, 9999).ToString(); //generating a random appointment number from 1000 to 9999
            return appointmentNumber;
        }

   } 
}