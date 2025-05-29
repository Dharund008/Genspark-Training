using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.Models;
using Hospital.Models.DTOs;

namespace Hospital.Interfaces
{
    public interface IDoctorSpecialityService
    {
        Task<DoctorSpeciality> GetDoctorSpecialityById(int id);
        Task<ICollection<DoctorSpeciality>> GetAllDoctorSpecialities();
        Task<DoctorSpeciality> AddDoctorSpeciality(DoctorSpecialityAddRequestDto doctorSpeciality);
        Task<DoctorSpeciality> UpdateDoctorSpeciality(int id, DoctorSpecialityUpdateRequestDto doctorSpeciality);
        Task<bool> DeleteDoctorSpeciality(int id);
    }
}
