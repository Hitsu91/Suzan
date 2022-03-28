using Suzan.Domain.Model;

namespace Suzan.Domain.DTOs.User;
public class UserProfileDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public Role Role { get; set; } = default;
}