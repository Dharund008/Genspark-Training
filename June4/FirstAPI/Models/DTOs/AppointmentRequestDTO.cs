namespace FirstAPI.Models.DTOs.DoctorSpecialities
{
    public class AppointmentRequestDTO
    {
        public string PatientEmail { get; set; } = string.Empty;
        public string PatientPassword { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public DateTime AppointmentTime { get; set; }
    }
}
