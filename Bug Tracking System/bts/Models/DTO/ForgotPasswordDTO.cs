using Bts.Models;
using System.ComponentModel.DataAnnotations;
namespace Bts.Models.DTO
{
    public class ForgotPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }

}