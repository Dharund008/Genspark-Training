using System.ComponentModel.DataAnnotations;
namespace NotifyAPI.Models.DTO
{
    public class UserRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

       
    }

}