using NotifyAPI.Models.DTO;
using NotifyAPI.Models;

namespace NotifyAPI.Interfaces
{
    public interface IUserService
    {
        public Task<User> AddUser(UserRequestDTO user);

        public Task<User> GetUser(string name);

    }
}