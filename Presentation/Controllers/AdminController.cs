using Business.Interfaces;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.Models;
using Presentation.ViewModels;

namespace Presentation.Controllers;

//[Authorize]
[Route("/admin")]
public class AdminController : Controller
{
    private readonly IStatusService _statusService;
    private readonly IUserService _userService;
    private readonly IClientService _clientService;
    private readonly IProjectService _projectService;

    public AdminController(IUserService userService, IClientService clientService, IStatusService statusService, IProjectService projectService)
    {
        _userService = userService;
        _clientService = clientService;
        _statusService = statusService;
        _projectService = projectService;
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

    [HttpGet]
    [Route("/projects")]
    public async Task<IActionResult> Projects()
    {
        var statuses = await _statusService.GetAllStatuses();
        var clients = await _clientService.GetAllClientsAsync();
        var members = await _userService.GetAllUsersAsync();
        var projects = await _projectService.GetAllProjectsAsync();

        var viewModel = new ProjectViewModel
        {
            AddProjectModel = new AddProjectModel
            {
                Statuses = statuses.Select(x => new SelectListItem
                {
                    Text = x.StatusName,
                    Value = x.StatusId.ToString(),
                }),

                Clients = clients.Select(x => new SelectListItem
                {
                    Text = x.ClientName,
                    Value = x.Id.ToString()
                }),

                Members = members.Select(x => new SelectListItem
                {
                    Text = $"{x.FirstName} {x.LastName}",
                    Value = x.Id.ToString()
                })
            },
            EditProjectModel = new EditProjectModel
            {
                Statuses = statuses.Select(x => new SelectListItem
                {
                    Text = x.StatusName,
                    Value = x.StatusId.ToString(),
                }),

                Clients = clients.Select(x => new SelectListItem
                {
                    Text = x.ClientName,
                    Value = x.Id.ToString()
                }),

                Members = members.Select(x => new SelectListItem
                {
                    Text = $"{x.FirstName} {x.LastName}",
                    Value = x.Id.ToString()
                })
            },
            Projects = projects
        };

        return View(viewModel);
    }

}
