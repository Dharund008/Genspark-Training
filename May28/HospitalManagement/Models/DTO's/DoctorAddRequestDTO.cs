using System;
using Hospital.Interfaces;


namespace Hospital.Models.DTOs.DoctorSpecialities
{
    public class DoctorAddRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<SpecialityAddRequestDto>? Specialities { get; set; }
        public float YearsOfExperience { get; set; }
    }
}