using NotifyAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotifyAPI.Repositories
{
    public interface IFileRepository
    {
        Task<FileDocument> Add(FileDocument document);
        Task<IEnumerable<FileDocument>> GetAll();
    }
}
