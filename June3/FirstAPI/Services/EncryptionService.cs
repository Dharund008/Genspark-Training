using System.Security.Cryptography;
using System.Text;
using FirstAPI.Interfaces;
using FirstAPI.Models;
/*
If a hash key is given, use it to hash the password (for login).
If not, generate a new key (for registration).
Always return the hashed password and the key used.
*/

namespace FirstAPI.Services
{
    public class EncryptionService : IEncryptionService
    {
        // EncryptData takes an EncryptModel (with Data and optionally HashKey)
        public async Task<EncryptModel> EncryptData(EncryptModel data)
        {
            HMACSHA256 hMACSHA256;
            // 1. If a HashKey is provided, use it to initialize HMACSHA256 (for password verification).
            if (data.HashKey != null)
                hMACSHA256 = new HMACSHA256(data.HashKey);
            // 2. If no HashKey is provided, generate a new one (for new user registration).
            else
                hMACSHA256 = new HMACSHA256();
            // 3. Compute the hash of the input data (password) using HMACSHA256.
            data.EncryptedData = hMACSHA256.ComputeHash(Encoding.UTF8.GetBytes(data.Data));
            // 4. Store the key used for hashing (important for verifying later).
            data.HashKey = hMACSHA256.Key;
            // 5. Return the EncryptModel with EncryptedData and HashKey set.
            return data;
        }
    }
}