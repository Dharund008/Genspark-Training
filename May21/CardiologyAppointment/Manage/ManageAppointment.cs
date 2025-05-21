using CardiologyAppointmentApp.Models;
using CardiologyAppointmentApp.Interfaces;


namespace CardiologyAppointmentApp.Manage
{
    public class ManageAppointment
    {
        private readonly IAppointmentService _service;

        public ManageAppointment(IAppointmentService service)
        {
            _service = service;
        }

        public void Start()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nWelcomme to Cardiology Appointing System!");
                Console.WriteLine("\n1. Add Appointment\n2. Search Appointments\n3. Exit");
                Console.Write("Choose an option: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        AddAppointment();
                        break;
                    case 2:
                        SearchAppointments();
                        break;
                    case 3:
                        exit = true;
                        Console.WriteLine("Exiting...GoodBYe! Have a nice Day!!!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
        }

        private void AddAppointment()
        {
            var app = new Appointment();

            Console.Write("Enter Patient Name: ");
            app.PatientName = Console.ReadLine();
            Console.Write("Enter Patient Age: ");
            app.PatientAge = int.Parse(Console.ReadLine());
            Console.Write("Enter Date (yyyy-mm-dd): ");
            app.AppointmentDate = DateTime.Parse(Console.ReadLine());
            Console.Write("Enter Reason: ");
            app.Reason = Console.ReadLine();

            int id = _service.AddAppointment(app);
            Console.WriteLine($"Appointment added successfully with ID: {id}");
        }

        private void SearchAppointments()
        {
            var model = new AppointmentSearchModel();

            Console.Write("Search by Name (or skip): ");
            var name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name)) model.PatientName = name;

            Console.Write("Search by Date (yyyy-mm-dd) (or skip): ");
            var date = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(date)) model.AppointmentDate = DateTime.Parse(date);

            Console.Write("Min Age (or skip): ");
            var min = Console.ReadLine();
            Console.Write("Max Age (or skip): ");
            var max = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(min) && !string.IsNullOrWhiteSpace(max))
            {
                model.AgeRange = new Range<int>
                {
                    MinVal = int.Parse(min),
                    MaxVal = int.Parse(max)
                };
            }

            var results = _service.SearchAppointments(model);
            if (results == null || results.Count == 0)
                Console.WriteLine("No results found.");
            else
            {
                foreach (var app in results)
                    Console.WriteLine($"[{app.Id}] {app.PatientName}, Age {app.PatientAge}, Date {app.AppointmentDate.ToShortDateString()}, Reason: {app.Reason}");
            }
        }
    }
}
