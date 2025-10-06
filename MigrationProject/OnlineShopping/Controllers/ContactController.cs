using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Online.Models;
using Online.Models.DTO;
using Online.Interfaces;
using Online.Contexts;
using Online.Services;


namespace Online.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly IMailService _mailService;

        public ContactController(IContactService contactService, IMailService mailService)
        {
            _contactService = contactService;
            _mailService = mailService;
        }

        [Authorize]
        [HttpPost("submit-form")]
        public async Task<IActionResult> SubmitContact([FromBody] SupportDTO contact)
        {
            try
            {
                var saved = await _contactService.AddContactAsync(contact);
                await _mailService.SendContactAutoReplyAsync(saved);
                return Ok(new { message = "Contact message submitted and email sent successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed: {ex.Message}" });
            }
        }
    }

}