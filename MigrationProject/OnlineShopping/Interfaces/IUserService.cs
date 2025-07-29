using Online.Models;
using Online.Repositories;
using Online.Models.DTO;

namespace Online.Interfaces
{
    public interface IUserService
    {
        //Task<User> CreateUserAsync(RegisterDTO register);
        public Task<bool> IsUsernameExists(string Username);
        public Task<User> GetByIdAsync(int Id);
        Task<User> UpdateUserAsync(int Id, UpdateUserDTO updateUser); //can update username and password

        Task<User> ChangeUsernameAsync(int Id, UsernameDTO name);
        Task<User> ChangePhoneAsync(int Id, PhoneDTO name);
        Task<User> ChangeAddressAsync(int Id, AddressDTO name);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<UserDTO>> GetAllAsync();
        
    }
}