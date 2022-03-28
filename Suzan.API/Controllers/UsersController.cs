using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Suzan.Application.Services.UserService;
using Suzan.Domain.DTOs.User;
using Suzan.Domain.Model;

namespace Suzan.API.Controllers;

[ApiController]
[Route("api/v1/users")]
[Authorize(Roles = nameof(Role.Admin))]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<User>>> GetAll([FromQuery] PaginationFilter paginationFilter)
    {
        var result = await _userService.GetAllPaged(paginationFilter);
        return Ok(result);
    }

    [HttpPut("update-role/{id:guid}")]
    public async Task<ActionResult<UserProfileDto>> UpdateUser([FromRoute] Guid id,
        [FromBody] UserUpdateRoleDto userUpdateRole)
    {
        return Ok(await _userService.UpdateRole(id, userUpdateRole.NewRole));
    }
}