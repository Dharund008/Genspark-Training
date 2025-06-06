using System.ComponentModel.DataAnnotations;
namespace NotifyAPI.Models.DTO
{
    public class LoginResponse
    {
        public string Username { get; set; } = string.Empty;
        public string? Token { get; set; }
    }
}