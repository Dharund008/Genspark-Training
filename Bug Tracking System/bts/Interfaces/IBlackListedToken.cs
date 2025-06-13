using Bts.Models;
using Bts.Models.DTO;
namespace Bts.Interfaces
{
    public interface IBlacklistedTokenRepository
    {
        Task AddTokenAsync(BlacklistedToken token);
        Task<bool> IsTokenBlacklistedAsync(string token);
    }

}