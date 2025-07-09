using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Twitter.Models
{
    public class UserProfile
    {
        //[Key, ForeignKey("User")]
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        public User? User { get; set; } // navigation property
        // navigating back to User because of the foreign key.

    }
}