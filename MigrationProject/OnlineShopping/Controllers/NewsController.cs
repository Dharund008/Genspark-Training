using Microsoft.AspNetCore.Mvc;
using Online.Interfaces;
using Online.Models;
using Online.Models.DTO;

namespace Online.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpPost("upload-news-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                //_logger.LogWarning("File not selected in UploadScreenshot");
                return BadRequest("File not selected");
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var savePath = Path.Combine("wwwroot/News", fileName);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            //_logger.LogInformation("Uploaded screenshot {FileName}", fileName);
            return Ok(new { url = $"/NewsImage/{fileName}" });
        }

        [HttpPost]
        public async Task<IActionResult> AddNews([FromBody] AddNewsDTO news)
        {
            try
            {
                var created = await _newsService.AddNewsAsync(news);
                return Ok(created);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews([FromQuery] int id)
        {
            var result = await _newsService.DeleteNewsAsync(id);
            if (!result)
                return NotFound(new { message = "News item not found." });

            return Ok(new { message = "Deleted successfully." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNews()
        {
            var news = await _newsService.GetAllNewsAsync();
            return Ok(news);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNewsById([FromQuery] int id)
        {
            try
            {
                var news = await _newsService.GetNewsByIdAsync(id);
                return Ok(news);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
