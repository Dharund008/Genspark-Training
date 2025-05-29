using System;
namespace Hospital.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public float YearsOfExperience { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }

        public ICollection<DoctorSpeciality>? DoctorSpecialities { get; set; }
    }
}