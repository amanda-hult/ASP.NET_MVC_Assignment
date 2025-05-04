using Business.Interfaces;
using Business.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.Models;
using Presentation.ViewModels;

namespace Presentation.Controllers;

[Authorize]
[Route("/admin")]
public class AdminController(IUserService userService, IClientService clientService, IStatusService statusService, IProjectService projectService) : Controller
{
    private readonly IStatusService _statusService = statusService;
    private readonly IUserService _userService = userService;
    private readonly IClientService _clientService = clientService;
    private readonly IProjectService _projectService = projectService;

    [AllowAnonymous]
    [HttpGet]
    public IActionResult AdminLogIn()
    {
        return View();
    }

    [AllowAnonymous]
    [Route("/denied")]
    public IActionResult Denied()
    {
        return View();
    }


    #region Members
    [Authorize(Roles = "Admin")]
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
    #endregion

    #region Clients
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
    #endregion

    #region Projects
    [HttpGet]
    [Route("/projects")]
    public async Task<IActionResult> Projects()
    {
        var statuses = await _statusService.GetAllStatuses();
        var clients = await _clientService.GetAllClientsAsync();
        var projects = await _projectService.GetAllProjectsAsync();

        var preselectedMembers = new Dictionary<int, List<BasicUserModel>>();
        foreach (var project in projects)
        {
            var memberIds = project.ProjectUsers.Select(x => x.UserId).ToList();
            var members = await _userService.GetBasicUsersByIdAsync(memberIds);
            preselectedMembers[project.ProjectId] = members.ToList();
        }

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
                })
            },
            Projects = projects,
            PreselectedMembers = preselectedMembers
        };

        return View(viewModel);
    }
    #endregion
}
