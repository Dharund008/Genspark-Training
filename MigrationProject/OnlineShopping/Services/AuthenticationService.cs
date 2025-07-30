
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Online.Models;
using Online.Models.DTO;
using Online.Interfaces;
using Online.Contexts;

namespace Online.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly MigrationContext _context;
        private readonly IEncryptionService _encryptionService;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IRepository<int, User> _userRepository;

        public AuthenticationService(MigrationContext context, IEncryptionService encryptionService, ITokenService tokenService, IRepository<int, User> userRepository,
                                        IUserService userService)
        {
            _context = context;
            _encryptionService = encryptionService;
            _tokenService = tokenService;
            _userRepository = userRepository;
            _userService = userService;
        }

        public async Task<LoginResponseDTO> UserLogin(UserLoginDTO user)
        {
            try
            {
                var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username.ToLower());
                if (dbUser == null)
                {
                    throw new Exception("No such user");
                }
                var encryptedData = await _encryptionService.EncryptData(new EncryptModel
                {
                    Data = user.Password,
                });
                bool isPasswordValid = _encryptionService.VerifyPassword(user.Password, dbUser.Password);
                if (!isPasswordValid)
                {
                    throw new Exception("Invalid password");
                }
                var token = await _tokenService.GenerateToken(dbUser);
                return new LoginResponseDTO
                {
                    Username = user.Username,
                    Token = token,
                };
            }
            catch (Exception ex)
            {
                throw new Exception("User is notFound or Failed to retrieve token", ex);
            }
        }

        public async Task<User> CreateUserAsync(RegisterDTO reg)
        {
            try
            {
                 if (reg == null)
                {
                    throw new ArgumentNullException(nameof(reg));
                }
                var usernameExists = await _userService.IsUsernameExists(reg.Username);
                if (usernameExists)
                {
                    throw new Exception("User with this email already exists.");
                }
                var data = new EncryptModel { Data = reg.Password };
                var encryptedPassword = await _encryptionService.EncryptData(data);

                var user = new User
                {
                    Username = reg.Username.ToLower(),
                    Password = encryptedPassword.EncryptedString,
                    CustomerPhone = reg.CustomerPhone,
                    CustomerEmail = reg.CustomerEmail,
                    CustomerAddress = reg.CustomerAddress
                };
                user = await _userRepository.AddAsync(user);
                Console.WriteLine($"AuthService : New User Registered : {user.Username} ");
                if (user == null)
                {
                    throw new Exception("Failed to add User");
                }
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Authentication Service : {ex.Message}");
                throw new Exception($"Failed to add User {ex.Message}");
            }
        }
    }
}