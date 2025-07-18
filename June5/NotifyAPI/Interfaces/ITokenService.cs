using NotifyAPI.Models;

namespace NotifyAPI.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(Register user);
    }
}