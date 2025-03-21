using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;

namespace Presentation.Controllers;

//[Authorize]
[Route("/admin")]
public class AdminController : Controller
{

    //[AllowAnonymous]
    [HttpGet]
    public IActionResult AdminLogIn()
    {
        return View();
    }


    //[Authorize(Roles = "admin")]
    [HttpGet]
    [Route("/members")]
    public IActionResult Members()
    {
        var viewModel = new MembersViewModel();
        return View(viewModel);
    }


    //[Authorize(Roles = "admin")]
    [HttpGet]
    [Route("/clients")]
    public IActionResult Clients()
    {
        return View();
    }
}
