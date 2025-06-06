using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FirstAPI.Interfaces;
using FirstAPI.Models;
using Microsoft.IdentityModel.Tokens;

/*
Reads secret key from config.
Builds claims for the user.
Creates and signs a JWT token with claims and expiry.
Returns the token string.
*/

namespace FirstAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _securityKey;
        public TokenService(IConfiguration configuration)
        {
            // 1. Read the JWT secret key from configuration and create a symmetric security key.
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Keys:JwtTokenKey"]));
        }
        public async Task<string> GenerateToken(User user)//from authentication service - verified user!
            // 1. Read the JWT secret key from configuration and create a symmetric security key.
            // This is already done in the constructor, so we can directly use _securityKey here.
        {
            // 2. Create claims for the user (username and role).
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };
            // 3. Create signing credentials using the security key and HMAC SHA256.
            var creds = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256Signature);

            // 4. Create a token descriptor with claims, expiry, and signing credentials.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            // 5. Use JwtSecurityTokenHandler to create and write the token as a string.
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}