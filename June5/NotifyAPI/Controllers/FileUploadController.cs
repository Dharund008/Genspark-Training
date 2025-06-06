using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotifyAPI.Models.DTO;
using NotifyAPI.Services;
using NotifyAPI.Misc;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace NotifyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : ControllerBase
    {
        private readonly FileService _fileService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public FileUploadController(FileService fileService, IHubContext<NotificationHub> hubContext)
        {
            _fileService = fileService;
            _hubContext = hubContext;
        }

        [HttpPost("file-upload")]
        [Authorize(Policy = "HRPolicy")]
        public async Task<IActionResult> Upload([FromForm] FileUploadDTO dto)
        {
            var uploadedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _fileService.UploadFileAsync(dto, uploadedBy);
            await _hubContext.Clients.All.SendAsync("ReceivedNotification", "A new file was uploaded by HR.");
            return Ok("File uploaded successfully.");
        }
    }
}
