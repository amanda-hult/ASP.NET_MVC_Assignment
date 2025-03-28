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
    private readonly IClientService _clientService;

    public AdminController(IUserService userService, IClientService clientService)
    {
        _userService = userService;
        _clientService = clientService;
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
    public async Task<IActionResult> Clients()
    {
        var clients = await _clientService.GetAllClientsAsync();

        var viewModel = new ClientViewModel
        {
            Clients = clients
        };

        return View(viewModel);
    }


}
