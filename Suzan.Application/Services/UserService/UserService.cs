using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Suzan.Application.Data;
using Suzan.Application.Helpers;
using Suzan.Domain.DTOs.User;
using Suzan.Domain.Exceptions;
using Suzan.Domain.Model;

namespace Suzan.Application.Services.UserService;

public class UserService : IUserService
{
    private readonly DataContext _ctx;
    private readonly IMapper _mapper;
    private readonly DbSet<User> _users;

    public UserService(IMapper mapper, DataContext ctx)
    {
        _mapper = mapper;
        _ctx = ctx;
        _users = ctx.Users;
    }

    public async Task<PagedResponse<UserProfileDto>> GetAllPaged(PaginationFilter filter)
    {
        var users = await _users.Paginate(filter)
            .Select(u => _mapper.Map<UserProfileDto>(u))
            .ToListAsync();

        var count = await _users.CountAsync();

        return PaginationHelper.CreatePagedResponse(users, filter, count);
    }

    public async Task<UserProfileDto> UpdateRole(Guid id, Role newRole)
    {
        var user = await _users.FindAsync(id);
        if (user is null)
            throw new ModelValidationException(
                "Update User error",
                StatusCodes.Status404NotFound,
                nameof(id),
                $"Cannot find user with id {id}");

        user.Role = newRole;
        await _ctx.SaveChangesAsync();
        return _mapper.Map<UserProfileDto>(user);
    }
}