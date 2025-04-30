using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Models.Users;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
namespace Business.Services;

public class AuthService(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager) : IAuthService
{
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly UserManager<UserEntity> _userManager = userManager;

    public async Task<bool> SignInAsync(SignInModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        return result.Succeeded;
    }

    public async Task<IdentityResult> SignInExternalAsync(ExternalUserModel model, ExternalLoginInfo info)
    {
        // factory
        var userEntity = new UserEntity
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            UserName = model.UserName,
        };

        var result = await _userManager.CreateAsync(userEntity);
        if (result.Succeeded)
        {
            await _userManager.AddLoginAsync(userEntity, info);
            await _signInManager.SignInAsync(userEntity, isPersistent: false);
        }
        return result;
    }

    public async Task<bool> SignUpAsync(SignUpModel model)
    {
        var userEntity = UserFactory.Create(model);

        var result = await _userManager.CreateAsync(userEntity, model.Password);
        return result.Succeeded;
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

}
