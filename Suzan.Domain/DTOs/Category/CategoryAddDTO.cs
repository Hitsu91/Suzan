using System.ComponentModel.DataAnnotations;

namespace Suzan.Domain.DTOs.Category;

public class CategoryAddDto
{
    [Required] public string Name { get; set; } = string.Empty;
}