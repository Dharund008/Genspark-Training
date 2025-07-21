using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Video.Interfaces;

namespace Video.Services
{
    public class BlobStorageService : IBlobService
    {
        private readonly BlobContainerClient _containerClient;
        private readonly IConfiguration _configuration;

        public BlobStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
            var connString = _configuration["AzureBlobStorage:ConnectionString"];
            var containerName = _configuration["AzureBlobStorage:ContainerName"];

            _containerClient = new BlobContainerClient(connString, containerName);

        }

        public async Task<string> UploadFileAsync(Stream stream, IFormFile file)
        {
            //var blobName = $"{Guid.NewGuid()}-{file.FileName}";
            var blobClient = _containerClient.GetBlobClient(file.FileName);

            //await using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);
            return blobClient.Uri.ToString();

            //return blobName;
        }

        // public Task<Uri> GenerateSasUriAsync(string blobName)
        // {
        //     if (!_useSas)
        //         throw new InvalidOperationException("SAS generation is disabled in configuration.");

        //     var blobClient = _containerClient.GetBlobClient(blobName);
        //     var sasBuilder = new BlobSasBuilder
        //     {
        //         BlobContainerName = blobClient.BlobContainerName,
        //         BlobName = blobClient.Name,
        //         Resource = "b",
        //         ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
        //     };
        //     sasBuilder.SetPermissions(BlobSasPermissions.Read);

        //     var sasUri = blobClient.GenerateSasUri(sasBuilder);
        //     return Task.FromResult(sasUri);
        // }
    }
}
