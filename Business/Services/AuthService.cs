using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
namespace Business.Services;

public class AuthService : IAuthService
{
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly UserManager<UserEntity> _userManager;

    public AuthService(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<bool> SignInAsync(SignInModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        return result.Succeeded;
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
