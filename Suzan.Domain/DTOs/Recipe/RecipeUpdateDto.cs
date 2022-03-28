namespace Suzan.Domain.DTOs.Recipe;

public class RecipeUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
}