using System.Threading.Tasks;
using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using FirstAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FirstAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;

        public PatientController(IPatientService patientService, IAppointmentService appointmentService)
        {
            _patientService = patientService;
            _appointmentService = appointmentService;

        }

        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient([FromBody] PatientAddRequestDTO pdto)
        {
            try
            {
                var newPatient = await _patientService.AddPatient(pdto);
                if (newPatient != null)
                    return Created("", newPatient);
                return BadRequest("Unable to process request at this moment");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("add-appointment")]
        [Authorize(Roles ="Patient")]
        public async Task<ActionResult<Patient>> AddAppointment([FromBody] AppointmentRequestDTO request)
        {
            var app = await _appointmentService.AddAppointment(request);
            if (app == null)
            {
                return BadRequest("Failed booking appointment");
            }
            else
            {
                return Ok(app);
            }

        }

    }
}