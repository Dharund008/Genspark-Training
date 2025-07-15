using System;
using Twitter.Models;

namespace Twitter.DTOs
{
    public class UpdateUserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}