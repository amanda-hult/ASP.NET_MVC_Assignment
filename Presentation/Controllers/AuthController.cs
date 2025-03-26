using System.Diagnostics;
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
        ViewBag.ErrorMessage = null;
        var viewModel = new SignUpViewModel();
        return View(viewModel);
    }

    [Route("/signup")]
    [HttpPost]
    public async Task<IActionResult> SignUp([Bind(Prefix = "")] SignUpModel model)
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
        var result = await _authService.SignUpAsync(model);
        if (result)
            return LocalRedirect("~/");

        ViewBag.ErrorMessage = "";
        return View();
    }


    [Route("/signin")]
    [HttpGet]
    public IActionResult SignIn(string returnUrl = "~/")
    {
        ViewBag.ErrorMessage = "";
        ViewBag.ReturnUrl = returnUrl;
        var viewModel = new SignInViewModel();
        return View(viewModel);
    }

    [Route("/signin")]
    [HttpPost]
    public async Task<IActionResult> SignIn(SignInViewModel viewModel, string returnUrl = "~/")
    {
        ViewBag.ErrorMessage = "";

        var model = viewModel.Form;

        //SignInViewModel signInViewModel = new()
        //{
        //    Form = viewModel.Form
        //};

        if (!ModelState.IsValid)
        {
            foreach (var kvp in ModelState)
            {
                var key = kvp.Key;
                var errors = kvp.Value?.Errors;
                foreach (var error in errors!)
                {
                    Debug.WriteLine($"Field: {key}, Error: {error.ErrorMessage}");
                }
            }
            Debug.WriteLine("Modellen är ogiltig.");
        }
        if (ModelState.IsValid)
        {
            var result = await _authService.SignInAsync(model);

            Debug.WriteLine($"Login secceeded: {result}");

            if (result)
                return LocalRedirect(returnUrl);

        }

        //bool exists = await _authService.Exists(model.Email);
        //if (!exists)
        //{
        //    ViewBag.ErrorMessage = "The email is not registered. Plese create an account to sign in.";
        //    return View(viewModel);
        //}

        ViewBag.ErrorMessage = "Incorrect email or password.";
        return View(viewModel);
    }

    public new async Task<IActionResult> SignOut()
    {
        await _authService.SignOutAsync();
        return RedirectToAction("SignIn", "Auth");
    }

}
