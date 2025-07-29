
using Online.Models;
using Online.Models.DTO;
using System.Threading.Tasks;


namespace Online.Interfaces
{
    public interface IEncryptionService
    {
        Task<EncryptModel> EncryptData(EncryptModel data);
        bool VerifyPassword(string input, string hashedPassword);

    }
}