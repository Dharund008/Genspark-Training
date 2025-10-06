namespace Hospital.Models
{
    public class Appointment
    {
        [Key] //annotating that this is the primary key 
        ///because the context scripts will look for a property called Id as the primary key.
        public string AppointmentNumber { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string Status { get; set; } = string.Empty;

        [ForeignKey("PatientId")] // annotating that this is the foreign key
        // as the scripts look for a property called PatientId as the foreign key.
        public Patient? Patient { get; set; }

        [ForeignKey("DoctorId")] // annotating that this is the foreign key
        public Doctor? Doctor { get; set; }
    }
}