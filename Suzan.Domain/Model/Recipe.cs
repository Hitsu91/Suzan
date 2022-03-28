namespace Suzan.Domain.Model;

public class Recipe
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Category? Category { get; set; }

    public User? Author { get; set; }
}