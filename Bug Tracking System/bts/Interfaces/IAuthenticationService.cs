using Bts.Models.DTO;

namespace Bts.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<LoginResponse> UserLogin(Login user);
        public Task<bool> Logout(string token);
    }
}