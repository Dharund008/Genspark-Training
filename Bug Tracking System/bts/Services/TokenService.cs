using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Bts.Interfaces;
using Bts.Models;   

namespace Bts.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _securityKey;

        public TokenService(IConfiguration configuration)
        {
            // 1. Read the JWT secret key from configuration and create a symmetric security key.
            var jwtKey = configuration["Keys:JwtTokenKey"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new Exception("JwtTokenKey:JWT token key is missing in configuration.");
            }
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        }

        public async Task<string> GenerateToken(User reg)
        {
            List<Claim> claims = new List<Claim> //claiming with respective username(email) and role
            {
                new Claim(ClaimTypes.Email, reg.Username),
                new Claim("MyApp_Id", reg.Id),
                new Claim(ClaimTypes.NameIdentifier, reg.Username),
                new Claim(ClaimTypes.Role, reg.Role)
            };

            //signing credentials using the security key and HMAC SHA256 algorithm
            var creds = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256Signature);

            //token descriptor 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds
            };
            //token handler to create and write the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }
}