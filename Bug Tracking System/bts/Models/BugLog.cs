namespace Bts.Models;

public class BugLog
{
    public int Id { get; set; }
    public int BugId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string PerformedBy { get; set; } = string.Empty;
    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;
}
