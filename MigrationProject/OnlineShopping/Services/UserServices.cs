using Microsoft.EntityFrameworkCore;
using Online.Models;
using Online.Models.DTO;
using Online.Interfaces;
using Online.Contexts;
using Online.Repositories;


namespace Online.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<int, User> _userRepository;
        private readonly MigrationContext _context;
        private readonly IUserService _userService;

        public UserService(IRepository<int, User> userRepository, MigrationContext context, IUserService userService)
        {
            _userRepository = userRepository;
            _context = context;
            _userService = userService;
        }

        public async Task<bool> IsUsernameExists(string Username)
        {
            return await _userRepository.AnyAsync(x => x.Username == Username);
        }

        public async Task<User> CreateUserAsync(RegisterDTO reg)
        {
            try
            {
                if (reg == null)
                {
                    throw new ArgumentNullException(nameof(reg));
                }
                var usernameExists = await IsUsernameExists(reg.Username);
                if (usernameExists)
                {
                    throw new Exception("User with this email already exists.");
                }

                var user = new User
                {
                    Username = reg.Username,
                    Password = reg.Password //need to hash(encrypt : decrypt before storing it!)
                };
                user = await _userRepository.AddAsync(user);
                Console.WriteLine($"New User Registered : {user.Username} ");
                if (user == null)
                {
                    throw new Exception("Failed to add User");
                }
                return user;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in User Service : {ex.Message}");
                throw new Exception($"Failed to add User {ex.Message}");
            }
        }
    }
}
