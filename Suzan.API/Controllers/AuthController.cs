using Microsoft.AspNetCore.Mvc;
using Suzan.Application.Services.AuthService;
using Suzan.Domain.DTOs.User;
using Suzan.Domain.Exceptions;

namespace Suzan.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserRegistrationResponse>> Register(UserRegistrationRequest user)
    {
        try
        {
            var result = await _authService.Register(user);
            return StatusCode(StatusCodes.Status201Created, result);
        }
        catch (ModelValidationException e)
        {
            return StatusCode(e.StatusCode, e.Errors);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserLoginResponse>> Login(UserLoginRequest user)
    {
        try
        {
            var result = await _authService.Login(user);
            return Ok(result);
        }
        catch (ModelValidationException ex)
        {
            return StatusCode(ex.StatusCode, ex.Errors);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}