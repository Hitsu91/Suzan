namespace Suzan.Domain.DTOs.Recipe;

public class RecipeAddDto
{
    public string Name { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
}