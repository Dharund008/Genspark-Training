using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Twitter.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// DateTime.Now - causes issue in postgre sql because,it uses local system time.
        /// whereas,
        /// postgresql expects with timezone (UTC)
        /// it expects all timestamp with time zone fields to be UTC
        /// DateTime.UtcNow
        /// </summary>

        public UserProfile? Profile { get; set; } // one-one relationship : one user can have only one profile
        public ICollection<Post> Posts { get; set; }
        public ICollection<Like> Likes { get; set; }
    }
}