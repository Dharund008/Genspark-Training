using System;
using Twitter.Models;

namespace Twitter.DTOs
{
    public class ResponseUserClass
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}