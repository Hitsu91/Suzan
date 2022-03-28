using Suzan.Domain.Model;

namespace Suzan.Application.Services.IdentityService;

public interface IIdentityService
{
    public Guid GetUserId();
    Task<User> GetCurrentUser();
    Task<T> GetCurrentUserAs<T>();
}