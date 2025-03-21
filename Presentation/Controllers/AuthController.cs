using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.ViewModels;

namespace Presentation.Controllers;

public class AuthController : Controller
{
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    private readonly IAuthService _authService;


    [Route("/signup")]
    [HttpGet]
    public IActionResult SignUp()
    {
        var viewModel = new SignUpViewModel();
        return View(viewModel);
    }

    [Route("/signup")]
    [HttpPost]
    public IActionResult SignUp([Bind(Prefix = "AddProjectModel")] SignUpModel model)
    {
        if (!ModelState.IsValid)
        {
            SignUpViewModel viewModel = new()
            {
                Form = model
            };
            return View(viewModel);
        }

        //_accountService.ExistsAsync(model);
        //_accountService.CreateAccountAsync(model);

        return View();
    }


    [Route("/signin")]
    [HttpGet]
    public IActionResult SignIn(string returnUrl = "/")
    {
        ViewBag.ReturnUrl = returnUrl;
        var viewModel = new SignInViewModel();
        return View(viewModel);
    }

    [Route("/signin")]
    [HttpPost]
    public async Task<IActionResult> SignIn(SignInModel model, string returnUrl = "/")
    {
        ViewData["ErrorMessage"] = string.Empty;

        SignInViewModel viewModel = new()
        {
            Form = model
        };

        if (!ModelState.IsValid)
        {
            ViewData["ErrorMessage"] = "Incorrect email or password.";
            return View(viewModel);
        }

        //bool exists = await _authService.Exists(model.Email);
        //if (!exists)
        //{
        //    ViewData["ErrorMessage"] = "The email is not registered. Plese create an account to sign in.";
        //    return View(viewModel);
        //}

        var result = await _authService.SignInAsync(model);
        if (result)
            return LocalRedirect(returnUrl);

        ViewData["ErrorMessage"] = "Incorrect email or password.";
        return View(viewModel);
    }





    //public new async Task<IActionResult> SignOut()
    //{
    //    await _signInManager.SignOutAsync();
    //    return RedirectToAction("SignIn", "Auth");
    //}
}
