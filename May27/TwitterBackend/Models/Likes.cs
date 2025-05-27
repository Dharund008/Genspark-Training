using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Twitter.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Status { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("PostId")]
        public Post? Post { get; set; }
    }
}