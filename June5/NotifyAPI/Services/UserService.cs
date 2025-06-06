using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotifyAPI.Contexts;
using NotifyAPI.Interfaces;
using NotifyAPI.Models;
using NotifyAPI.Models.DTO;
using NotifyAPI.Repositories;
using NotifyAPI.Misc;

namespace NotifyAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ManagementContext _context;
        private readonly IMapper _mapper;
        private readonly IRepository<string, User> _userrepository;
        private readonly IRepository<string, Register> _registerRepository;
        private readonly IEncryptionService _encryptionService;

        public UserService(
            ManagementContext context,
            IMapper mapper,
            IRepository<string, User> userrepository,
            IRepository<string, Register> registerRepository,
            IEncryptionService encryptionService)
        {
            _context = context;
            _mapper = mapper;
            _userrepository = userrepository;
            _registerRepository = registerRepository;
            _encryptionService = encryptionService;
        }

        public async Task<User> AddUser(UserRequestDTO user)
        {
            try
            {
                // var existingUser = await _userrepository.Get(user.Name);
                // if (existingUser != null)
                // {
                //     throw new Exception("User with this name already exists");
                // }

                var register = _mapper.Map<UserRequestDTO, Register>(user);

                var encryptedData = await _encryptionService.EncryptData(new EncryptModel
                {
                    Data = user.Password,
                });
                register.Password = encryptedData.EncryptedData; // Store the encrypted password
                register.HashKey = encryptedData.HashKey; // Store the hash key for future encryption
                register.Role = "User";
                register = await _registerRepository.Add(register);

                var newUser = new User
                {
                    Name = user.Name,
                    Email = user.Email
                };
                newUser = await _userrepository.Add(newUser);
                if (newUser == null)
                {
                    throw new Exception("Failed to add New User");
                }
                return newUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in User services - Add: {ex.Message}");
                throw new Exception($"Failed to add User {ex.Message}");
            }
        }

        public async Task<User> GetUser(string email)
        {
            try
            {
                var newUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                //in services, u will be using db table name (set in context) rather than class name 
                //but not same in repo ...
                return newUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in User Service - Get Email: {ex.Message}");
                throw new Exception("Failed to Get User", ex);
            }
        }
    }
}
