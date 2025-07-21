using BlobAPI.Models;
using BlobAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BlobAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly BlobStorageService _blobStorageService;
        private readonly ILogger<FilesController> _logger;

        public FilesController(BlobStorageService blobStorageService, ILogger<FilesController> logger)
        {
            _blobStorageService  = blobStorageService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Download(string fileName)
        {
            try
            {
                _logger.LogInformation("Download request received for file: {FileName}", fileName);
                var stream = await _blobStorageService.DownloadFile(fileName);
                if (stream == null)
                {
                    _logger.LogWarning("File not found: {FileName}", fileName);
                    return NotFound();
                }
                _logger.LogInformation("File downloaded successfully: {FileName}", fileName);
                return File(stream, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while downloading file: {FileName}", fileName);
                return StatusCode(500, "Internal server error");
            }
        }

        [Consumes("multipart/form-data")]
        
        [HttpPost("upload")]

        public async Task<IActionResult> Upload([FromForm] UploadRequestDto request)
        {
            try
            {
                if (request.File == null || request.File.Length == 0)
                {
                    _logger.LogWarning("Upload request with no file");
                    return BadRequest("No file to upload");
                }
                _logger.LogInformation("Upload request received for file: {FileName}", request.File.FileName);
                using var stream = request.File.OpenReadStream();
                await _blobStorageService.UploadFile(stream, request.File.FileName);
                _logger.LogInformation("File uploaded successfully: {FileName}", request.File.FileName);
                return Ok("File uploaded");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while uploading file");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}