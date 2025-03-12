using Business.Interfaces;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class RoleService(IRoleRepository roleRepository) : IRoleService
{
    private readonly IRoleRepository _roleRepository = roleRepository;

    // CREATE

    // READ
    public async Task<RoleEntity> GetRoleEntityByIdAsync(int id)
    {
        var roleEntity = await _roleRepository.GetAsync(x => x.RoleId == id);

        if (roleEntity == null)
            throw new ArgumentNullException(nameof(roleEntity));

        return roleEntity;
    }

    // UPDATE

    // DELETE
}
