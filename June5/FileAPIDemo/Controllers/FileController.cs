using Microsoft.AspNetCore.Mvc;

namespace File.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "FileStorage");

        public FileController()
        {
            // Ensure the directory exists
            if (!Directory.Exists(_filePath))
                Directory.CreateDirectory(_filePath);
        }

        // POST: /file/upload
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var fullPath = Path.Combine(_filePath, file.FileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { message = "File uploaded successfully!", fileName = file.FileName });
        }

        // GET: /file/download/{filename}
        [HttpGet("download/{filename}")]
        public IActionResult Download(string filename)
        {
            var fullPath = Path.Combine(_filePath, filename);

            if (!System.IO.File.Exists(fullPath))
                return NotFound("File not found.");

            var contentType = "application/octet-stream";
            return PhysicalFile(fullPath, contentType, filename);
        }
    }
}
