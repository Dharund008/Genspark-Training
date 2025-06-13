using Bts.Contexts;
using Bts.Interfaces;
using Bts.Models;
using Bts.Models.DTO;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;


using Microsoft.Extensions.Logging;
namespace Bts.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenService _tokenService;
        private readonly IEncryptionService _encryptionService;

        private readonly IRepository<string, User> _userRepository;
        private readonly IBlacklistedTokenRepository _tokenBlacklist;
        private readonly BugContext _context;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(ITokenService tokenService, IEncryptionService encryptionService,
                IRepository<string, User> userRepository, IBlacklistedTokenRepository _to, BugContext context,
                ILogger<AuthenticationService> logger)
        {
            _tokenService = tokenService;
            _encryptionService = encryptionService;
            _userRepository = userRepository;
            _tokenBlacklist = _to;
            _context = context;
            _logger = logger;
        }

        public async Task<LoginResponse> UserLogin(Login user)
        {
            try
            {
                var dbUser = await _userRepository.GetById(user.Username);
                if (dbUser == null)
                {
                    throw new Exception("No such user");
                }
                if (dbUser.Role == "DEVELOPER")
                {
                    var dev = await _context.Developers
                        .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(d => d.Email == user.Username);

                    if (dev == null || dev.IsDeleted)
                        throw new Exception("Developer account is deactivated.");
                }
                if (dbUser.Role == "TESTER")
                {
                    var dev = await _context.Testers
                        .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(t => t.Email == user.Username);

                    if (dev == null || dev.IsDeleted)
                        throw new Exception("Tester account is deactivated.");
                }
                // if (dbUser.Role == "DEVELOPER" || dbUser.Role == "TESTER")
                // {
                //     var userAccount = await _context.Set<BaseUser>()
                //         .IgnoreQueryFilters()
                //         .FirstOrDefaultAsync(u => u.Email == user.Username);

                //     if (userAccount == null || userAccount.IsDeleted)
                //         throw new Exception($"{dbUser.Role} account is deactivated.");
                // }

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
                return new LoginResponse
                {
                    Username = user.Username,
                    Token = token,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user login");
                throw new Exception("User is deactived or Failed to retrieve token", ex);
            }
        }
        public async Task<bool> Logout(string token)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var expiry = jwtToken.ValidTo;

            var blacklistedToken = new BlacklistedToken
            {
                Token = token,
                ExpiryDate = expiry.ToUniversalTime()
            };

            await _tokenBlacklist.AddTokenAsync(blacklistedToken);
            _logger.LogInformation("Token blacklisted: {Token}", token);
            return true;
        }
    }
}
