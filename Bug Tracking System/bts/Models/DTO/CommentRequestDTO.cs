using System.ComponentModel.DataAnnotations;

namespace Bts.Models.DTO
{
    public class CommentRequestDTO
    {
        [Required]
        public int BugId { get; set; }

        [Required]
        public string Message { get; set; } = string.Empty;
    }
}
