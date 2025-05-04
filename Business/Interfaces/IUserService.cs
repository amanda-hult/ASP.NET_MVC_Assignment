using Business.Models.Users;

namespace Business.Interfaces;

public interface IUserService
{
    Task<int> CreateUserAsync(UserCreateModel model);
    Task<IEnumerable<UserModel>> GetAllUsersAsync();
    Task<IEnumerable<BasicUserModel>> GetBasicUsersByStringAsync(string term);
    Task<IEnumerable<BasicUserModel>> GetBasicUsersByIdAsync(List<string> ids);
    Task<int> UpdateUserAsync(UserEditModel model);
    Task<int> DeleteUserAsync(string id);
}
