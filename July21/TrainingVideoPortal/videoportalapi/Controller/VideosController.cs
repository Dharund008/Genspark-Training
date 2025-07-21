using Microsoft.AspNetCore.Mvc;
using Video.Interfaces;
using Video.Models;

namespace Video.Controllers
{
    [ApiController]
    [Route("api/videos")]
    public class VideosController : ControllerBase
    {
        private readonly ITrainingVideoService _videoService;

        public VideosController(ITrainingVideoService videoService)
        {
            _videoService = videoService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // if (request == null)
            // {
            //     throw new Exception("No file uploaded.");
            // }

            var video = await _videoService.UploadAsync(
                request
            );

            // return CreatedAtAction(
            //     nameof(GetAll),
            //     new { id = video.Id },
            //     video
            // );
            return Ok(video);
        }

        [HttpGet("getallvideos")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var videos = await _videoService.GetAllAsync();
                return Ok(videos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
