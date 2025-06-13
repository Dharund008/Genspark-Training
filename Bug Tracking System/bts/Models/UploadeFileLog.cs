using System.ComponentModel.DataAnnotations;

namespace Bts.Models
{
    public class UploadedFileLog
    {
        public int Id { get; set; }

        [Required]
        public string DeveloperId { get; set; } = string.Empty;

        [Required]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public string FilePath { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
