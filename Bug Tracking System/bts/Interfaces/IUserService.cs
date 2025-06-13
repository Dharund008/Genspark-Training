using Bts.Models;
using Bts.Models.DTO;

namespace Bts.Interfaces
{

    public interface IUserService
    {
        Task<string?> GeneratePasswordResetTokenAsync(string email);

        Task<bool> ResetPasswordAsync(ResetPasswordDTO dto);

    }
    

}