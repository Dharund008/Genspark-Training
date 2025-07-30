using System.ComponentModel.DataAnnotations;
using Online.Models;

namespace Online.Models.DTO
{
    public class AddNewsDTO
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string ShortDescription { get; set; } = string.Empty;

        [Required]
        public string? Image { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;
    }
}