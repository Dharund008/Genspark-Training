using Bts.Models.DTO;
using Bts.Models;
using System.Threading.Tasks;

namespace Bts.Interfaces
{
    public interface IEncryptionService
    {
        Task<EncryptModel> EncryptData(EncryptModel data);
        bool VerifyPassword(string input, string hashedPassword);

        Task<string> GenerateId(string str);
    }
}
