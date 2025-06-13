using Bts.Models;
using System.ComponentModel.DataAnnotations;
namespace Bts.Models.DTO
{
    public class ResetPasswordDTO
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }

}