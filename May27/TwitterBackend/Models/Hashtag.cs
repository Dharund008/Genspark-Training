using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Twitter.Models
{
    public class Hashtag
    {
        public int Id { get; set; }
        public string Tag { get; set; } = string.Empty;

        public ICollection<Post>? Posts { get; set; }
    }
}