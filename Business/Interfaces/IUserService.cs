using Business.Models.Users;
using Data.Entities;

namespace Business.Interfaces;

public interface IUserService
{
    Task<bool> CreateUserAsync(UserCreateModel model);
    Task<IEnumerable<UserModel>> GetAllUsersAsync();
    Task<IEnumerable<UserModel>> GetUsersByIdAsync(List<string> ids);
    Task<List<UserEntity>> GetUserEntitiesByIdAsync(List<string> ids);
}
