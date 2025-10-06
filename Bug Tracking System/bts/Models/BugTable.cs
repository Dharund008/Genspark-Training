using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bts.Models
{
    public class Bug
    {   

            public int Id { get; set; }

            [Required, MaxLength(200)]
            public string Title { get; set; } = string.Empty;

            [Required, MaxLength(2000)]
            public string Description { get; set; } = string.Empty;

            public string? ScreenshotUrl { get; set; } = null;

            [Required]
            public BugPriority Priority { get; set; }

            [Required]
            public BugStatus Status { get; set; }

            [ForeignKey("CreatedByTester")]
            [Required]
            public string CreatedBy { get; set; } = string.Empty;
            [JsonIgnore]
            public Tester? CreatedByTester { get; set; }

            [ForeignKey("AssignedToDeveloper")]
            public string? AssignedTo { get; set; }
            [JsonIgnore]
            public Developer? AssignedToDeveloper { get; set; }

            public bool IsDeleted { get; set; } = false;
            public string? Reason { get; set; } = null;

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
            public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

        public enum BugPriority
        {
            Low,
            Medium,
            High,
            Critical
        }

        public enum BugStatus
        {
            New, Assigned, InProgress, Fixed, Retesting, Verified, Reopened, Closed
        }
}
