using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;

namespace FirstAPI.Interfaces
{
    public interface IPatientService
    {
        public Task<Patient> GetPatient(string name);
        public Task<Patient> AddPatient(PatientAddRequestDTO patient);
    }
}
