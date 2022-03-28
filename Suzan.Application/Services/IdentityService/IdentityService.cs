using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Suzan.Application.Data;
using Suzan.Domain.Exceptions;
using Suzan.Domain.Model;

namespace Suzan.Application.Services.IdentityService;

public class IdentityService : IIdentityService
{
    private readonly DataContext _ctx;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityService(DataContext ctx, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _ctx = ctx;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUserId()
    {
        var nameIdentifierClaim = _httpContextAccessor
            .HttpContext?
            .User
            .FindFirstValue(ClaimTypes.NameIdentifier);

        if (nameIdentifierClaim is null)
        {
            throw new ModelValidationException("Authentication State", StatusCodes.Status401Unauthorized);
        }

        return Guid.Parse(nameIdentifierClaim);
    }


    public async Task<User> GetCurrentUser()
    {
        var loggedUser = await _ctx.Users.FindAsync(GetUserId());
        if (loggedUser is null)
        {
            throw new ModelValidationException("Authentication State", StatusCodes.Status401Unauthorized);
        }
        return loggedUser;
    }

    public async Task<T> GetCurrentUserAs<T>()
    {
        return _mapper.Map<T>(await GetCurrentUser());
    }
}