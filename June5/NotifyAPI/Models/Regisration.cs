using System.ComponentModel.DataAnnotations;

namespace NotifyAPI.Models
{
    public class Register
    {
        [Key]
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public byte[]? Password { get; set; }

        public byte[]? HashKey { get; set; }

        public User? User { get; set; }
        public HRAdmin? HRAdmin { get; set; }
    }
}