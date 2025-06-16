namespace Bts.Models.DTO
{
    public class UpdateBugPatchDTO
    {
        public string? Description { get; set; }
        public BugPriority Priority { get; set; }
        public string? ScreenshotUrl { get; set; }
    }
}