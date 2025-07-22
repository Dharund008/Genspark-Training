using Video.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Video.Interfaces
{
    public interface ITrainingVideoService
    {
        Task<TrainingVideo> UploadAsync(UploadRequest request);
        Task<IEnumerable<TrainingVideo>> GetAllAsync();
    }
}
