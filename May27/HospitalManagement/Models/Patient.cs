namespace Hospital.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public ICollection<Appointment>? Appointments { get; set; } // navigation property
                                                                    // one - many : patient has more than one appointments 
                                                                    // this property links each patient to all their appointments

        
    }
}