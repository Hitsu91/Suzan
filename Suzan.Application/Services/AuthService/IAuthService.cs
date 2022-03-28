using Suzan.Domain.DTOs.User;

namespace Suzan.Application.Services.AuthService;

public interface IAuthService
{
    Task<UserRegistrationResponse> Register(UserRegistrationRequest user);

    Task<UserLoginResponse> Login(UserLoginRequest user);
}