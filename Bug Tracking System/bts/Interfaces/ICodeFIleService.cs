namespace Bts.Interfaces
{
    public interface ICodeFileService
    {
        Task UploadFile(Stream stream, string fileName, string containerName);

        Task<Stream?> DownloadFile(string fileName, string containerName);
    }
}