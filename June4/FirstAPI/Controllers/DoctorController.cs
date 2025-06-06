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
    [Route("/api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IRepository<string, Appointmnet> _appointmentRepository;

        public DoctorController(IDoctorService doctorService, IRepository<string, Appointmnet> appointmentRepository)
        {
            _doctorService = doctorService;
            _appointmentRepository = appointmentRepository;
        }

        [HttpGet]
        [Authorize] //any one can access this endpoint irrespecitve of their roles.
        public async Task<ActionResult<IEnumerable<DoctorsBySpecialityResponseDto>>> GetDoctors(string speciality)
        {
            var result = await _doctorService.GetDoctorsBySpeciality(speciality);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<Doctor>> PostDoctor([FromBody] DoctorAddRequestDto doctor)
        {
            try
            {
                var newDoctor = await _doctorService.AddDoctor(doctor);
                if (newDoctor != null)
                    return Created("", newDoctor);
                return BadRequest("Unable to process request at this moment");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpDelete("delete-appointment/{appointmentNumber}")]
        [Authorize(Policy = "ExperiencedDoctorOnly")]
        public async Task<IActionResult> DeleteAppointment(string appointmentNumber)
        {
            try
            {
                var appointment = await _appointmentRepository.Get(appointmentNumber);
                if (appointment == null)
                    return NotFound("Appointment not found");

                // Soft delete: set status to "Cancelled"
                appointment.Status = "Cancelled";
                await _appointmentRepository.Update(appointmentNumber, appointment);

                return Ok("Appointment cancelled successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}