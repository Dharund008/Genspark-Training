using Video.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Video.Interfaces
{
    public interface ITrainingVideoService
    {
        Task<TrainingVideo> UploadAsync(IFormFile file, string title, string description);
        Task<IEnumerable<TrainingVideo>> GetAllAsync();
    }
}
