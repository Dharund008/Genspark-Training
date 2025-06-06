using Microsoft.AspNetCore.Http;

namespace NotifyAPI.Models.DTO
{
    public class FileUploadDTO
    {
        public IFormFile File { get; set; }
    }
}