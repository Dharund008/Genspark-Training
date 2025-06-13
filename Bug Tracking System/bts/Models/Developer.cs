using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bts.Models
{
    public class Developer
    {
        public string Id { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;

        [JsonIgnore]
        public User? User { get; set; }

        public ICollection<Bug>? Bugs { get; set; }
    }
}