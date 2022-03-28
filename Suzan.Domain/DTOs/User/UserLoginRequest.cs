namespace Suzan.Domain.DTOs.User;

public class UserLoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public void Deconstruct(out string username, out string password)
    {
        username = Username;
        password = Password;
    }
}
