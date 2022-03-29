using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace Suzan.Application.Services.BlobService;

public class BlobService : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> UploadFile(string containerName, Stream file,
        string fileName, string contentType)
    {
        var blobClient = GetBlobClient(containerName, fileName);
        var options = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = contentType
            }
        };
        await blobClient.UploadAsync(file, options);
        return fileName;
    }

    public async Task DeleteFile(string containerName, string fileName)
    {
        var blobClient = GetBlobClient(containerName, fileName);
        await blobClient.DeleteAsync();
    }

    public async Task<string> UploadFile(string containerName, IFormFile file, string? newName)
    {
        var fileName = file.FileName;
        if (newName is not null)
        {
            var extension = Path.GetExtension(file.FileName);
            fileName = $"{newName}{extension}";
        }

        return await UploadFile(
            containerName,
            file.OpenReadStream(),
            fileName,
            file.ContentType
        );
    }

    private BlobClient GetBlobClient(string containerName, string fileName)
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = blobContainer.GetBlobClient(fileName);
        return blobClient;
    }
}