using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.ViewModels;

namespace Presentation.Controllers;

public class AuthController : Controller
{
    [Route("/signup")]
    [HttpGet]
    public IActionResult SignUp()
    {
        var viewModel = new SignUpViewModel();
        return View(viewModel);
    }

    [Route("/signup")]
    [HttpPost]
    public IActionResult SignUp([Bind(Prefix ="Form")] SignUpModel model)
    {
        if (!ModelState.IsValid)
        {
            SignUpViewModel viewModel = new()
            {
                Form = model
            };
            return View(viewModel);
        }

        //_accountService.CreateAccountAsync(model);

        return View();
    }


    [Route("/signin")]
    [HttpGet]
    public IActionResult SignIn()
    {
        var viewModel = new SignInViewModel();
        return View(viewModel);
    }

    [Route("/signin")]
    [HttpPost]
    public IActionResult SignIn(SignInModel model)
    {
        if (!ModelState.IsValid)
        {
            SignInViewModel viewModel = new()
            {
                Form = model
            };
            return View(viewModel);
        }

        return View();
    }



    public new IActionResult SignOut()
    {
        return View();
    }
}
