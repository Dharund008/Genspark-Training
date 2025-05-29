using System;

namespace Hospital.Models.DTOs
{
    public class DoctorSpecialityUpdateRequestDto
    {
        public int DoctorId { get; set; }
        public int SpecialityId { get; set; }
    }
}
