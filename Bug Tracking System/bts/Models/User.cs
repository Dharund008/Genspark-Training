using System.ComponentModel.DataAnnotations;

namespace Bts.Models
{
    public class User
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;


        public Admin? Admin { get; set; }
        public Developer? Developer { get; set; }
        public Tester? Tester { get; set; }
    }
}