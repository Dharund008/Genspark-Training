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
    public class PatientService : IPatientService
    {
        private readonly PatientRepository _patientRepository;

        public PatientService(PatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<Patient> GetPatientById(int id)
        {
            var patient = await _patientRepository.Get(id);
            if (patient == null)
                throw new Exception("Patient not found");
            return patient;
        }

        public async Task<ICollection<Patient>> GetAllPatients()
        {
            var patients = await _patientRepository.GetAll();
            return patients.ToList();
        }

        public async Task<Patient> AddPatient(PatientAddRequestDto patientDto)
        {
            if (patientDto == null)
                throw new ArgumentNullException(nameof(patientDto));

            var patient = new Patient
            {
                Name = patientDto.Name,
                Status = patientDto.Status,
                DateOfBirth = patientDto.DateOfBirth,
                Address = patientDto.Address
            };

            await _patientRepository.Add(patient);
            return patient;
        }

        public async Task<Patient> UpdatePatient(int id, PatientUpdateRequestDto patientDto)
        {
            if (patientDto == null)
                throw new ArgumentNullException(nameof(patientDto));

            var patient = await _patientRepository.Get(id);
            if (patient == null)
                throw new Exception("Patient not found");

            patient.Name = patientDto.Name;
            patient.Status = patientDto.Status;
            patient.DateOfBirth = patientDto.DateOfBirth;
            patient.Address = patientDto.Address;

            await _patientRepository.Update(id, patient);
            return patient;
        }

        public async Task<bool> DeletePatient(int id)
        {
            var patient = await _patientRepository.Get(id);
            if (patient == null)
                throw new Exception("Patient not found");

            await _patientRepository.Delete(id);
            return true;
        }
    }
}
