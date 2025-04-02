using Business.Models;
using Business.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace Business.Interfaces;

public interface IAuthService
{
    Task<bool> SignInAsync(SignInModel model);
    Task<IdentityResult> SignInExternalAsync(ExternalUserModel model, ExternalLoginInfo info);
    Task<bool> SignUpAsync(SignUpModel model);
    Task SignOutAsync();
}
