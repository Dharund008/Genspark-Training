using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Video.Models;
using Video.Interfaces;
using Video.Contexts;

namespace Video.Services
{
    public class TrainingVideoService : ITrainingVideoService
    {
        private readonly TrainingVideoContext _context;
        private readonly IBlobService _blobService;
        private readonly IConfiguration _configuration;

        public TrainingVideoService(
            TrainingVideoContext context,
            IBlobService blobService,
            IConfiguration configuration)
        {
            _context = context;
            _blobService = blobService;
            _configuration = configuration;
        }

        public async Task<TrainingVideo> UploadAsync(UploadRequest request)
        {
            using var stream = request.File.OpenReadStream();
            var url = await _blobService.UploadFileAsync(stream, request.File);

            //create metadata record
            var video = new TrainingVideo
            {
                Title = request.Title,
                Description = request.Description,
                UploadDate = DateTime.UtcNow,
                BlobUrl = url
            };

            //save to database
            _context.TrainingVideos.Add(video);
            await _context.SaveChangesAsync();

            return video;
        }

        public async Task<IEnumerable<TrainingVideo>> GetAllAsync()
        {
            var videos = await _context.TrainingVideos
                                 .OrderByDescending(v => v.UploadDate)
                                 .ToListAsync();
            return videos;
        }
    }
}
