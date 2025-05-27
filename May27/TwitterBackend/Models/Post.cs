using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Twitter.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;

        [ForeignKey("UserId")]
        public User? User { get; set; }

        public ICollection<Hashtag>? Hashtags { get; set; }
    }
}