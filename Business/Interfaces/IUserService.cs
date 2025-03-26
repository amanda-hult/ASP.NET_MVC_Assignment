using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces;

public interface IUserService
{
    Task<bool> CreateUserAsync(UserCreateModel model);
    Task<IEnumerable<UserModel>> GetAllUsersAsync();
}
