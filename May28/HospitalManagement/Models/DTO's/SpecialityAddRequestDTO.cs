using System;

namespace Hospital.Models.DTOs.DoctorSpecialities
{
    public class SpecialityAddRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
