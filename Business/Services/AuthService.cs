using Business.Models;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
namespace Business.Services;

public class AuthService
{
    private readonly SignInManager<UserEntity> _signInManager;

    public AuthService(SignInManager<UserEntity> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<bool> SignInAsync(SignInModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        return result.Succeeded;
    }

}
