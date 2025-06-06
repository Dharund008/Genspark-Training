using NotifyAPI.Models.DTO;
using NotifyAPI.Interfaces;
using NotifyAPI.Models;
using Microsoft.Extensions.Logging;

namespace NotifyAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenService _tokenService;
        private readonly IEncryptionService _encryptionService;
        private readonly IRepository<string, Register> _registerRepository;

        public AuthenticationService(
            ITokenService tokenService,
            IEncryptionService encryptionService,
            IRepository<string, Register> registerRepository)
        {
            _tokenService = tokenService;
            _encryptionService = encryptionService;
            _registerRepository = registerRepository;
        }

        public async Task<LoginResponse> UserLogin(Login registerer)
        {
            try
            {
                var dbuser = await _registerRepository.Get(registerer.Username);
                if (dbuser == null)
                {
                    Console.WriteLine($"User: {registerer.Username} not found");
                    throw new Exception("User not found");
                }
                if (dbuser.HashKey == null || dbuser.Password == null)
                {
                    Console.WriteLine($"User data incomplete for: {registerer.Username}");
                    throw new Exception("User data incomplete");
                }
                Console.WriteLine($"Encrypting password: {registerer.Password}, with hashKey: {dbuser.HashKey}");
                var encryptedData = await _encryptionService.EncryptData(new EncryptModel
                {
                    Data = registerer.Password,
                    HashKey = dbuser.HashKey // Use the stored hash key(user in db) for encryption
                });

                if (encryptedData.EncryptedData == null)
                {
                    Console.WriteLine("Encryption failed");
                    throw new Exception("Encryption failed");
                }
                if (encryptedData.EncryptedData.Length != dbuser.Password.Length)
                {
                    Console.WriteLine("Password length mismatch");
                    throw new Exception("Password length mismatch");
                }

                for (int i = 0; i < encryptedData.EncryptedData.Length; i++)
                {
                    if (encryptedData.EncryptedData[i] != dbuser.Password[i])
                    {
                        Console.WriteLine($"Password mismatch for user: {registerer.Username}");
                        throw new Exception("Invalid password");
                    }
                }

                var token = await _tokenService.GenerateToken(dbuser);
                return new LoginResponse
                {
                    Username = registerer.Username,
                    Token = token
                };
            }
            catch (Exception ex)
            {
                // Log the exception (if a logger is available)
                // _logger.LogError(ex, "Error during user login");
                Console.WriteLine($"Error in Authentication Service: {ex.Message}");
                throw new Exception("Failed to retrieve token", ex);
            }
        }
    }
}