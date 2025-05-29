using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.Models;
using Hospital.Models.DTOs;

namespace Hospital.Interfaces
{
    public interface IPatientService
    {
        Task<Patient> GetPatientById(int id);
        Task<ICollection<Patient>> GetAllPatients();
        Task<Patient> AddPatient(PatientAddRequestDto patient);
        Task<Patient> UpdatePatient(int id, PatientUpdateRequestDto patient);
        Task<bool> DeletePatient(int id);
    }
}
