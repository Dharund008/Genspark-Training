using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bts.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int BugId { get; set; } 
        [JsonIgnore]
        public Bug Bug { get; set; }

        public string UserId { get; set; } = string.Empty;
        [JsonIgnore]
        public User User { get; set; }

        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}