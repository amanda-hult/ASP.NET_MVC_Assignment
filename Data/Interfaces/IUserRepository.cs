using Data.Entities;

namespace Data.Interfaces;

public interface IUserRepository : IBaseRepository<UserEntity>
{
    Task<IEnumerable<UserEntity>> GetUsersByIdAsync(List<string> ids);
}
