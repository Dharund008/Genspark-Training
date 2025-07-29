
using BCrypt.Net;
using System.Security.Cryptography;
using System.Text;
using Online.Models;
using Online.Models.DTO;
using Online.Interfaces;


namespace Online.Services
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
    }
}