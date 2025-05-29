using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hospital.Interfaces;
using Hospital.Models;
using Hospital.Models.DTOs.DoctorSpecialities;
using Hospital.Repositories;

namespace Hospital.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly DoctorRepository _doctorRepository;
        private readonly SpecialityRepository _specialityRepository;

        public DoctorService(DoctorRepository doctorRepository, SpecialityRepository specialityRepository)
        {
            _doctorRepository = doctorRepository;
            _specialityRepository = specialityRepository;
        }

        public async Task<Doctor> GetDoctByName(string name)
        {
            var doctors = await _doctorRepository.GetAll();
            var doctor = doctors.FirstOrDefault(d => d.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (doctor == null)
                throw new Exception("Doctor not found with the given name");
            return doctor;
        }

        public async Task<ICollection<Doctor>> GetDoctorsBySpeciality(string speciality)
        {
            var specialities = await _specialityRepository.GetAll();
            var specialityEntity = specialities.FirstOrDefault(s => s.Name.Equals(speciality, StringComparison.OrdinalIgnoreCase));
            if (specialityEntity == null)
                throw new Exception("Speciality not found");

            var doctors = await _doctorRepository.GetAll();
            var doctorsWithSpeciality = doctors.Where(d => d.DoctorSpecialities != null && d.DoctorSpecialities.Any(ds => ds.SpecialityId == specialityEntity.Id)).ToList();

            return doctorsWithSpeciality;
        }

        public async Task<Doctor> AddDoctor(DoctorAddRequestDto doctorDto)
        {
            if (doctorDto == null)
                throw new ArgumentNullException(nameof(doctorDto));

            var doctor = new Doctor
            {
                Name = doctorDto.Name,
                YearsOfExperience = doctorDto.YearsOfExperience,
                Status = "Active",
                DoctorSpecialities = new List<DoctorSpeciality>()
            };

            if (doctorDto.Specialities != null)
            {
                foreach (var specialityDto in doctorDto.Specialities)
                {
                    var speciality = await _specialityRepository.Get(specialityDto.Id);
                    if (speciality == null)
                        throw new Exception($"Speciality with Id {specialityDto.Id} not found");

                    var doctorSpeciality = new DoctorSpeciality
                    {
                        Doctor = doctor,
                        Speciality = speciality,
                        DoctorId = doctor.Id,
                        SpecialityId = speciality.Id
                    };
                    doctor.DoctorSpecialities.Add(doctorSpeciality);
                }
            }

            await _doctorRepository.Add(doctor);
            return doctor;
        }
    }
}
