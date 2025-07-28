using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online.Models
{
    public class News
    {
        [Key]
        public int NewsId { get; set; }

        public int UserId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string ShortDescription { get; set; } = string.Empty;
        public string Image { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime? CreatedDate { get; set; }
        public int Status { get; set; }

        //properties
        public User? User { get; set; }
    }
    
}