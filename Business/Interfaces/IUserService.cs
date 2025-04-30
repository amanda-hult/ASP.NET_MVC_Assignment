using Business.Models.Users;
using Data.Entities;

namespace Business.Interfaces;

public interface IUserService
{
    Task<int> CreateUserAsync(UserCreateModel model);
    Task<IEnumerable<UserModel>> GetAllUsersAsync();
    Task<IEnumerable<UserModel>> GetUsersByIdAsync(List<string> ids);
    Task<IEnumerable<BasicUserModel>> GetBasicUsersByStringAsync(string term);
    Task<IEnumerable<BasicUserModel>> GetBasicUsersByIdAsync(List<string> ids);
    Task<List<UserEntity>> GetUserEntitiesByIdAsync(List<string> ids);
    Task<int> UpdateUserAsync(UserEditModel model);
    Task<int> DeleteUserAsync(string id);
}
