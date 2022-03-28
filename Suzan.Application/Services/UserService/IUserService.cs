using Suzan.Domain.DTOs.User;
using Suzan.Domain.Model;

namespace Suzan.Application.Services.UserService;

public interface IUserService
{
    Task<PagedResponse<UserProfileDto>> GetAllPaged(PaginationFilter filter);
    Task<UserProfileDto> UpdateRole(Guid id, Role newRole);
}