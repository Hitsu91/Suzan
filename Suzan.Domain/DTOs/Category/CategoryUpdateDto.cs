using System.ComponentModel.DataAnnotations;

namespace Suzan.Domain.DTOs.Category;

public class CategoryUpdateDto
{
    [Required] public string Name { get; set; } = string.Empty;
}