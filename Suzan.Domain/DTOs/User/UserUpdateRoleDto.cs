using Suzan.Domain.Model;

namespace Suzan.Domain.DTOs.User;

public class UserUpdateRoleDto
{
    public Role NewRole { get; set; }
}