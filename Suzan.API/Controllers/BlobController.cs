using Microsoft.AspNetCore.Mvc;
using Suzan.Application.Services.BlobService;
using Suzan.Domain.DTOs.UploadFile;

namespace Suzan.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BlobController : ControllerBase
{
    private readonly IBlobService _blobService;

    public BlobController(IBlobService blobService)
    {
        _blobService = blobService;
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> UploadFile(string id, [FromForm] UploadFileRequest fileRequest)
    {
        var extension = Path.GetExtension(fileRequest.File.FileName);
        await _blobService.UploadFile(
            "dishes",
            fileRequest.File,
            id
        );
        return Ok();
    }

    [HttpDelete("delete/{fileName}")]
    public async Task<IActionResult> DeleteFile(string fileName)
    {
        await _blobService.DeleteFile("dishes", fileName);
        return Ok();
    }
}