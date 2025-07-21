namespace Video.Interfaces
{
    public interface IBlobService
    {
        Task<string> UploadFileAsync(Stream stream, IFormFile file);
       // Task<Uri> GenerateSasUriAsync(string blobName);
    }
}