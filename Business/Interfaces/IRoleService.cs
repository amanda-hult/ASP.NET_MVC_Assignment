using Data.Entities;

namespace Business.Interfaces;

public interface IRoleService
{
    Task<RoleEntity> GetRoleEntityByIdAsync(int id);
}