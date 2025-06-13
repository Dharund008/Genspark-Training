using System.ComponentModel.DataAnnotations;
namespace Bts.Models.DTO
{
    public class LoginResponse
    {
        public string Username { get; set; } = string.Empty;
        public string? Token { get; set; }
    }
}