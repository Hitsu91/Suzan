using System.ComponentModel.DataAnnotations;

namespace Suzan.Domain.DTOs.User;

public class UserRegistrationRequest
{
    [MinLength(4), MaxLength(15)]
    public string Username { get; set; } = string.Empty;
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;
    public void Deconstruct(out string username, out string password)
    {
        username = Username;
        password = Password;
    }
}
