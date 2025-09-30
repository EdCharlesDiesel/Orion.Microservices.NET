namespace Orion.DataAccess.Postgres.Services;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(IFormFile file, string folder, string fileName);
    Task DeleteFileAsync(string fileUrl);
    Task<bool> FileExistsAsync(string fileUrl);
    Task<Stream> GetFileStreamAsync(string fileUrl);
}