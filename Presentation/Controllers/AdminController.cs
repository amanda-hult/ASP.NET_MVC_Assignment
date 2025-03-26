using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;

namespace Presentation.Controllers;

//[Authorize]
[Route("/admin")]
public class AdminController : Controller
{
    private readonly IUserService _userService;

    public AdminController(IUserService userService)
    {
        _userService = userService;
    }

    //[AllowAnonymous]
    [HttpGet]
    public IActionResult AdminLogIn()
    {
        return View();
    }

    //[AllowAnonymous]
    [Route("denied")]
    public IActionResult Denied()
    {
        return View();
    }


    //[Authorize(Roles = "admin")]
    [HttpGet]
    [Route("/members")]
    public async Task<IActionResult> Members()
    {
        var members = await _userService.GetAllUsersAsync();

        var viewModel = new MembersViewModel
        {
            Members = members
        };

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
