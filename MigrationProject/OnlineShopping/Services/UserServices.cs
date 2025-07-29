using Microsoft.EntityFrameworkCore;
using Online.Models;
using Online.Models.DTO;
using Online.Interfaces;
using Online.Contexts;
using Online.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace Online.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<int, User> _userRepository;
        private readonly MigrationContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserService(IRepository<int, User> userRepository, MigrationContext context, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> IsUsernameExists(string Username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == Username);
            if (user == null)
            {
                return false;
            }
            return true;
            //return await _userRepository.AnyAsync(x => x.Username == Username);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            if (id == 0)
            {
                throw new Exception("User-Id not valid");
            }
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return user;
        }

        public async Task<User> UpdateUserAsync(int Id, UpdateUserDTO updateUser)
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.FindFirst("MyApp_Id")?.Value;
            var user = await _userRepository.GetByIdAsync(Id);
            if (user == null && (user.UserId != int.Parse(currentUser)))
            {
                throw new Exception("User not found or not current User!");
            }
            //patch update
            if (updateUser.Username != null)
            {
                user.Username = updateUser.Username;
            }
            //_userRepository.Update(Id,)
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> ChangeUsernameAsync(int Id, UsernameDTO name)
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.FindFirst("MyApp_Id")?.Value;
            var user = await _userRepository.GetByIdAsync(Id);
            if (user == null && (user.UserId != int.Parse(currentUser)))
            {
                throw new Exception("User not found or not current User!");
            }

            if (name.Username != null)
            {
                user.Username = name.Username;
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> ChangePhoneAsync(int Id, PhoneDTO name)
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.FindFirst("MyApp_Id")?.Value;
            var user = await _userRepository.GetByIdAsync(Id);
            if (user == null && (user.UserId != int.Parse(currentUser)))
            {
                throw new Exception("User not found or not current User!");
            }

            if (name.CustomerPhone != null)
            {
                user.CustomerPhone = name.CustomerPhone;
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> ChangeAddressAsync(int Id, AddressDTO name)
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.FindFirst("MyApp_Id")?.Value;
            var user = await _userRepository.GetByIdAsync(Id);
            if (user == null && (user.UserId != int.Parse(currentUser)))
            {
                throw new Exception("User not found or not current User!");
            }

            if (name.CustomerAddress != null)
            {
                user.CustomerAddress = name.CustomerAddress;
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }



        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(user => new UserDTO
            {
                UserId = user.UserId,
                Username = user.Username,
            });
        }
    }
}
