using System;

namespace NotifyAPI.Models
{
    public class FileDocument
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string UploadedBy { get; set; } = string.Empty; // Username or HRAdmin name
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}