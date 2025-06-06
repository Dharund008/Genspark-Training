using NotifyAPI.Models;

namespace NotifyAPI.Interfaces
{
    public interface IEncryptionService
    {
        public Task<EncryptModel> EncryptData(EncryptModel data);
    }
}
// This interface defines a contract for an encryption service that provides a method to encrypt data.