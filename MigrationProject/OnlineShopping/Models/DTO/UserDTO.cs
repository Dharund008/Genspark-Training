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
        [Phone]
        public string CustomerPhone { get; set; } = string.Empty;

        [EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
    }

    public class UpdateUserDTO
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
        [Required]
        public string Username { get; set; } = string.Empty;
    }
    public class PhoneDTO
    {
        [Required]
        [Phone]
        public string CustomerPhone { get; set; } = string.Empty;
    }
    public class AddressDTO
    {
        [Required]
        public string CustomerAddress { get; set; } = string.Empty;
    }

    public class UserDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
    }

    public class LoginResponseDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}