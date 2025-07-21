using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Video.Models
{
    public class UploadRequest
    {
        [Required] public string Title { get; set; }
        [Required] public string Description { get; set; }
        [Required] public IFormFile File { get; set; }
    }
}
