using System.ComponentModel.DataAnnotations;
namespace Bts.Models.DTO
{
    public class Login
    {
        [Required(ErrorMessage = "Username is manditory")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is manditory")]
        public string Password { get; set; } = string.Empty;
    }
}