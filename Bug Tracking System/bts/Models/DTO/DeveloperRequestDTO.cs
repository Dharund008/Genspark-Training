using System.ComponentModel.DataAnnotations; //make sure to include this namespace for data annotations
namespace Bts.Models.DTO
{
    public class DeveloperRequestDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}