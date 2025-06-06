using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NotifyAPI.Interfaces;
using NotifyAPI.Models;

namespace NotifyAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _securityKey;

        public TokenService(IConfiguration configuration)
        {
            // 1. Read the JWT secret key from configuration and create a symmetric security key.
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Keys:JwtTokenKey"]));
        }

        public async Task<string> GenerateToken(Register reg)
        {
            List<Claim> claims = new List<Claim> //claiming with respective username(email) and role
            {
                new Claim(ClaimTypes.NameIdentifier, reg.Username),
                new Claim(ClaimTypes.Role, reg.Role)
            };

            //signing credentials using the security key and HMAC SHA256 algorithm
            var creds = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256Signature);

            //token descriptor 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            //token handler to create and write the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }
}