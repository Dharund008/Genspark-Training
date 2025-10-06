using System.ComponentModel.DataAnnotations;
using Online.Models;

namespace Online.Models.DTO
{
    public class SupportDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Message { get; set; } = string.Empty;
    }
}