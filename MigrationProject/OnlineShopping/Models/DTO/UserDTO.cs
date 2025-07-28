using System.ComponentModel.DataAnnotations;
using Online.Models;

namespace Online.Models.DTO
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Username is manditory")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is manditory")]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class UpdateUserDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class UsernameDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
    }

    public class UserDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}