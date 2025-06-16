using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bts.Models
{
    public class User
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;


        [JsonIgnore]
        public Admin? Admin { get; set; }

        [JsonIgnore]
        public Developer? Developer { get; set; }

        [JsonIgnore]
        public Tester? Tester { get; set; }
    }
}