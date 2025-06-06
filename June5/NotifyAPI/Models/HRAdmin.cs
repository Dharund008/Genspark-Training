using System.ComponentModel.DataAnnotations;

namespace NotifyAPI.Models
{
    public class HRAdmin
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        public string? Department { get; set; } = string.Empty;

        public Register? Register { get; set; }
    }
}