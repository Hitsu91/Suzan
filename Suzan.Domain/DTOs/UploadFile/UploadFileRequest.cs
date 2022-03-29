using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Suzan.Domain.DTOs.UploadFile;

public class UploadFileRequest
{
    [Required] public IFormFile File { get; set; } = null!;
}