using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Bts.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Bts.Contexts;
using Bts.Models;

namespace Bts.Services
{
    public class CodeFileService : ICodeFileService
    {
        private readonly BugContext _context;
        private readonly ICurrentUserService _currentUserService;

        private readonly BlobServiceClient _containerClient;
        private readonly ILogger<CodeFileService> _logger;

        public CodeFileService(IConfiguration configuration, ILogger<CodeFileService> logger,
                               BugContext context, ICurrentUserService currentUserService)
        {
            _containerClient = new BlobServiceClient(configuration["AzureBlobStorage:ConnectionString"]);
            _logger = logger;
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task UploadFile(Stream fileStream, string fileName, string containerName)
        {
            try
            {
                var containerClient = _containerClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync();
                var blobClient = containerClient.GetBlobClient(fileName);
                await blobClient.UploadAsync(fileStream, overwrite: true);

                var developerId = _currentUserService.Id;
                var relativePath = $"/api/CodeFile/downloadfile?fileName={fileName}&containername={containerName}";

                
                Console.WriteLine($"Download URL: {relativePath}");

                 // Save to DB
                var log = new UploadedFileLog
                {
                    DeveloperId = developerId,
                    FileName = fileName,
                    FilePath = relativePath,
                    UploadedAt = DateTime.UtcNow
                };

                _context.UploadedFileLogs.Add(log);
                await _context.SaveChangesAsync();
                _logger.LogInformation("File {FileName} uploaded successfully to container {ContainerName}", fileName, containerName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file {FileName} to container {ContainerName}", fileName, containerName);
                throw;
            }
        }
        public async Task<Stream?> DownloadFile(string fileName, string containerName)
        {
            var containerClient = _containerClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            _logger.LogInformation("Attempting to download file {FileName} from container {ContainerName}", fileName, containerName);
            
            if (await blobClient.ExistsAsync())
            {
                var downloadInfo = await blobClient.DownloadStreamingAsync();
                return downloadInfo.Value.Content;
            }
            
            return null;
        }
    }
}

