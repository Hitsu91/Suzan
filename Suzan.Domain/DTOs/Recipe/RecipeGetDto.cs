using Suzan.Domain.DTOs.Category;
using Suzan.Domain.DTOs.User;

namespace Suzan.Domain.DTOs.Recipe;


public class RecipeGetDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public CategoryGetDto? Category { get; set; }
    public AuthorDto? Author { get; set; }
}