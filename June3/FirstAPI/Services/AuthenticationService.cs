using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using Microsoft.Extensions.Logging;

/*
Receives login request with username and password.
Looks up user in the database.
Encrypts the provided password using the stored hash key.
Compares encrypted password with stored password.
If match, generates JWT token.
Returns username and token.
*/

namespace FirstAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenService _tokenService;
        private readonly IEncryptionService _encryptionService;
        private readonly IRepository<string, User> _userRepository;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(ITokenService tokenService,
                                    IEncryptionService encryptionService,
                                    IRepository<string, User> userRepository,
                                    ILogger<AuthenticationService> logger)
        {
            _tokenService = tokenService;
            _encryptionService = encryptionService;
            _userRepository = userRepository;
            _logger = logger;
        }
        public async Task<UserLoginResponse> Login(UserLoginRequest user)
        {
            // 1. Get the user from the database using the username.
            var dbUser = await _userRepository.Get(user.Username);

            // 2. If user not found, log and throw exception.
            if (dbUser == null)
            {
                _logger.LogCritical("User not found");
                throw new Exception("No such user");
            }

            // 3. Encrypt the password provided in the login request using the stored hash key.
            var encryptedData = await _encryptionService.EncryptData(new EncryptModel
            {
                Data = user.Password,
                HashKey = dbUser.HashKey // Use the stored hash key(user in db) for encryption
            });

            // 4. Compare the encrypted password with the stored password character by character.
            for (int i = 0; i < encryptedData.EncryptedData.Length; i++)
            {
                if (encryptedData.EncryptedData[i] != dbUser.Password[i])
                //checking whether the encrypted data of requsted with the stored user password(enrypted)
                {
                    // If any character does not match, log and throw exception.
                    _logger.LogError("Password mismatch for user {Username}", user.Username);
                    // Log the error with username for better debugging.
                    // This is a security risk, so be cautious about logging sensitive information.
                }
                // else if (i == encryptedData.EncryptedData.Length - 1)
                // // If we reach the end of the loop without throwing an exception, it means the password matches.
                // {
                //     _logger.LogError("Invalid login attempt"); 
                //     throw new Exception("Invalid password"); 
                // }
            }

            // 5. If password matches, generate a JWT token for the user.
            var token = await _tokenService.GenerateToken(dbUser);

            // 6. Return the username and token in the response.
            return new UserLoginResponse
            {
                Username = user.Username,
                Token = token,
            };
        }
    }
}