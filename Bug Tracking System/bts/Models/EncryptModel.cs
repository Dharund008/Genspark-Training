namespace Bts.Models
{
    public class EncryptModel
    {
         public string? Data { get; set; }              // plain text password
        public string EncryptedString { get; set; } = string.Empty;  // bcrypt hashed password
    }
}