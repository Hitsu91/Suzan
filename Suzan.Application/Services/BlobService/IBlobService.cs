using Microsoft.AspNetCore.Http;

namespace Suzan.Application.Services.BlobService;

public interface IBlobService
{
    Task<string> UploadFile(string containerName, IFormFile file, string? newName);

    Task<string> UploadFile(string containerName, Stream file,
        string fileName, string contentType);

    Task DeleteFile(string containerName, string fileName);
}