using NotifyAPI.Models.DTO;

namespace NotifyAPI.Interfaces
{
     public interface IAuthenticationService
    {
        public Task<LoginResponse> UserLogin(Login user);
    }
}