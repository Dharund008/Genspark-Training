using System.ComponentModel.DataAnnotations;
using Bts.Models; 

namespace Bts.Models.DTO
{
    public class BugSubmissionDTO
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [EnumDataType(typeof(BugPriority))]
        public BugPriority Priority { get; set; }

        public string? ScreenshotUrl { get; set; } = null;
    }
}
