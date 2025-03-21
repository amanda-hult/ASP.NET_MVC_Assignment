using Business.Models;

namespace Business.Interfaces;

public interface IAuthService
{
    Task<bool> SignInAsync(SignInModel model);
}
