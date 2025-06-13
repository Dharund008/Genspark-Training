using BCrypt.Net;
using System.Security.Cryptography;
using System.Text;
using Bts.Models.DTO;
using Bts.Models;
using Bts.Interfaces;

namespace Bts.Services
{
    public class EncryptionService : IEncryptionService
    {
        public async Task<EncryptModel> EncryptData(EncryptModel data)
        {
            if (data == null || string.IsNullOrEmpty(data.Data))
            {
                throw new Exception("Data to encrypt cannot be null or empty.");
            }
            // Hash the password with BCrypt (salt is handled internally)
            string hashed = BCrypt.Net.BCrypt.HashPassword(data.Data);

            data.EncryptedString = hashed; // store hashed string
            return data;
        }

        public bool VerifyPassword(string input, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(input, hashedPassword);
        }

        public async Task<string> GenerateId(string pre)
        {
            string prefix = pre;
            string guidPart = Guid.NewGuid().ToString("N").Substring(0, 8); // 8 chars

            string result = $"{prefix}{guidPart}";

            return result;
        }
    }
}
// wwwroot/screenshots/153da9ec-8b2c-403e-845a-87a0e070d214.png