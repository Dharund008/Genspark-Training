using System;

namespace Hospital.Models.DTOs
{
    public class DoctorSpecialityAddRequestDto
    {
        public int DoctorId { get; set; }
        public int SpecialityId { get; set; }
    }
}
