using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;

namespace BlobAPI.Services
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _containerClinet;
        private readonly ILogger<BlobStorageService> _logger;

        public BlobStorageService(IConfiguration configuration, ILogger<BlobStorageService> logger)
        {
            _logger = logger;
            // var sasUrl = configuration["AzureBlob:ContainerSasUrl"];
            // _containerClinet = new BlobContainerClient(new Uri(sasUrl));
            // var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            var containerName = configuration["AzureBlobStorage:ContainerName"];
            // _containerClinet = new BlobContainerClient(connectionString, containerName);
            var keyVaultUri = configuration["AzureKeyVault:VaultUri"];
            
            // Authenticate using DefaultAzureCredential
            var client = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
 
            // Fetch the secret from Key Vault
            KeyVaultSecret secret = client.GetSecret("blobapi");
            var connectionString = secret.Value;
 
            _containerClinet = new BlobContainerClient(connectionString, containerName);
        }

        public async Task UploadFile(Stream fileStream,string fileName)
        {
            try
            {
                _logger.LogInformation("Uploading file: {FileName}", fileName);
                var blobClient = _containerClinet.GetBlobClient(fileName);
                await blobClient.UploadAsync(fileStream,overwrite:true);
                _logger.LogInformation("File uploaded successfully: {FileName}", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while uploading file: {FileName}", fileName);
                throw;
            }
        }

        public async Task<Stream> DownloadFile(string fileName)
        {
            try
            {
                _logger.LogInformation("Downloading file: {FileName}", fileName);
                var blobClient = _containerClinet?.GetBlobClient(fileName);
                if(await blobClient.ExistsAsync())
                {
                    var downloadInfor = await blobClient.DownloadStreamingAsync();
                    _logger.LogInformation("File downloaded successfully: {FileName}", fileName);
                    return downloadInfor.Value.Content;
                }
                _logger.LogWarning("File not found: {FileName}", fileName);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while downloading file: {FileName}", fileName);
                throw;
            }
        }
    }
}