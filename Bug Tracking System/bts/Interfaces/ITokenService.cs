using Bts.Models;

namespace Bts.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(User user);
    }
}