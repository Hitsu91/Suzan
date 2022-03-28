namespace Suzan.Domain.DTOs.User;

public class UserLoginResponse
{
    public string Token { get; init; }

    public UserLoginResponse(string token)
    {
        Token = token;
    }
}
