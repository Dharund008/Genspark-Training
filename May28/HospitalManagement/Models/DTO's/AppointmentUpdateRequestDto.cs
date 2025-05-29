using System;

namespace Hospital.Models.DTOs
{
    public class AppointmentUpdateRequestDto
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
