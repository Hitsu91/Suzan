namespace Suzan.Domain.DTOs.User;

public class UserLoginResponse
{
    public UserLoginResponse(string token)
    {
        Token = token;
    }

    public string Token { get; init; }
}