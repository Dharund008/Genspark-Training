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
    public class DoctorSpecialityService : IDoctorSpecialityService
    {
        private readonly DoctorSpecialRepository _doctorSpecialRepository;

        public DoctorSpecialityService(DoctorSpecialRepository doctorSpecialRepository)
        {
            _doctorSpecialRepository = doctorSpecialRepository;
        }

        public async Task<DoctorSpeciality> GetDoctorSpecialityById(int id)
        {
            var doctorSpeciality = await _doctorSpecialRepository.Get(id);
            if (doctorSpeciality == null)
                throw new Exception("DoctorSpeciality not found");
            return doctorSpeciality;
        }

        public async Task<ICollection<DoctorSpeciality>> GetAllDoctorSpecialities()
        {
            var doctorSpecialities = await _doctorSpecialRepository.GetAll();
            return doctorSpecialities.ToList();
        }

        public async Task<DoctorSpeciality> AddDoctorSpeciality(DoctorSpecialityAddRequestDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var doctorSpeciality = new DoctorSpeciality
            {
                DoctorId = dto.DoctorId,
                SpecialityId = dto.SpecialityId
            };

            await _doctorSpecialRepository.Add(doctorSpeciality);
            return doctorSpeciality;
        }

        public async Task<DoctorSpeciality> UpdateDoctorSpeciality(int id, DoctorSpecialityUpdateRequestDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var doctorSpeciality = await _doctorSpecialRepository.Get(id);
            if (doctorSpeciality == null)
                throw new Exception("DoctorSpeciality not found");

            doctorSpeciality.DoctorId = dto.DoctorId;
            doctorSpeciality.SpecialityId = dto.SpecialityId;

            await _doctorSpecialRepository.Update(id, doctorSpeciality);
            return doctorSpeciality;
        }

        public async Task<bool> DeleteDoctorSpeciality(int id)
        {
            var doctorSpeciality = await _doctorSpecialRepository.Get(id);
            if (doctorSpeciality == null)
                throw new Exception("DoctorSpeciality not found");

            await _doctorSpecialRepository.Delete(id);
            return true;
        }
    }
}
