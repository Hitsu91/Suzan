namespace Suzan.Domain.Model;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
    public Role Role { get; set; } = Role.User;

    public List<Recipe> MyRecipes { get; set; } = new();
}