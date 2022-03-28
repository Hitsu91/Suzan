using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Suzan.Application.Data;
using Suzan.Application.Helpers;
using Suzan.Domain.DTOs.User;
using Suzan.Domain.Model;
using Suzan.Domain.Exceptions;

namespace Suzan.Application.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly DataContext _ctx;
    private readonly string _secret;

    public AuthService(DataContext ctx, IConfiguration config)
    {
        _ctx = ctx;
        _secret = config["AppSettings:Token"];
    }

    public async Task<UserRegistrationResponse> Register(UserRegistrationRequest user)
    {
        var (username, password) = user;

        if (UserExists(username))
        {
            throw new ModelValidationException(
                "Registration errors",
                StatusCodes.Status409Conflict,
                "username",
                $"Already exists user with username {username}");
        }

        var (hash, salt) = PasswordHashHelper.HashPassword(password);

        User newUser = new()
        {
            Username = username,
            PasswordHash = hash,
            PasswordSalt = salt
        };

        await _ctx.Users.AddAsync(newUser);
        await _ctx.SaveChangesAsync();

        return new UserRegistrationResponse { Username = username };
    }

    public async Task<UserLoginResponse> Login(UserLoginRequest request)
    {
        var (username, password) = request;
        var user = await _ctx.Users.FirstOrDefaultAsync(user => user.Username == username);

        if (user is null)
        {
            throw new ModelValidationException(
                "Login Error",
                StatusCodes.Status404NotFound,
                "username",
                $"Can't find user with username {username}");
        }

        if (!PasswordHashHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            throw new ModelValidationException(
                "Login Error",
                StatusCodes.Status404NotFound,
                "credentials",
                "Can't find user with specified credentials");
        }

        var token = CreateToken(user);
        return new UserLoginResponse(token);
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role.ToString()),
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = signingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    private bool UserExists(string username)
    {
        return _ctx.Users.Any(user => user.Username == username);
    }
}
