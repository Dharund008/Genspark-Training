
using Online.Models;

namespace Online.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(User user);
    }
}