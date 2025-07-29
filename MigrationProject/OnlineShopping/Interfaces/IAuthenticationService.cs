
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Online.Models;
using Online.Models.DTO;

namespace Online.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<LoginResponseDTO> UserLogin(UserLoginDTO user);
        Task<User> CreateUserAsync(RegisterDTO register); //regsiter
        //public Task<bool> Logout(string token);
    }
}