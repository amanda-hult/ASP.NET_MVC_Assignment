using System.Diagnostics;
using Business.Interfaces;
using Business.Models.Users;
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

    #region Members
    //[Authorize(Roles = "admin")]
    [HttpGet]
    [Route("/members")]
    public async Task<IActionResult> Members()
    {
        var members = await _userService.GetAllUsersAsync();

        //var editMemberModels = new Dictionary<string, EditMemberModel>();
        //foreach (var member in members)
        //{
        //    editMemberModels[member.Id] = new EditMemberModel
        //    {
        //        Id = member.Id,
        //        SelectedDay = member.DateOfBirth.HasValue ? member.DateOfBirth.Value.Day : 1,
        //        SelectedMonth = member.DateOfBirth.HasValue ? member.DateOfBirth.Value.Month : 1,
        //        SelectedYear = member.DateOfBirth.HasValue ? member.DateOfBirth.Value.Year : 1,
        //        Address = new AddressEditModel
        //        {
        //            StreetName = member.Address?.StreetName ?? string.Empty,
        //            StreetNumber = member.Address?.StreetNumber ?? string.Empty,
        //            PostalCode = member.Address?.PostalCode ?? string.Empty,
        //            City = member.Address?.City ?? string.Empty,
        //        }
        //    };
        //}

        var viewModel = new MembersViewModel
        {
            Members = members,

        };

        return View(viewModel);
    }
    #endregion

    #region Clients
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
    #endregion

    #region Projects
    [HttpGet]
    [Route("/projects")]
    public async Task<IActionResult> Projects()
    {
        var statuses = await _statusService.GetAllStatuses();
        var clients = await _clientService.GetAllClientsAsync();
        //var members = await _userService.GetAllUsersAsync();
        var projects = await _projectService.GetAllProjectsAsync();

        var preselectedMembers = new Dictionary<int, List<BasicUserModel>>();
        foreach (var project in projects)
        {
            var memberIds = project.ProjectUsers.Select(x => x.UserId).ToList();
            var members = await _userService.GetBasicUsersByIdAsync(memberIds);
            preselectedMembers[project.ProjectId] = members.ToList();
        }

        //var editProjectModel = new Dictionary<int, EditProjectModel>();
        //foreach (var project in projects)
        //{
        //    editProjectModel[project.ProjectId] = new EditProjectModel
        //    {
        //        Id = project.ProjectId,

        //        Statuses = statuses.Select(x => new SelectListItem
        //        {
        //            Text = x.StatusName,
        //            Value = x.StatusId.ToString(),
        //        }),

        //        Clients = clients.Select(x => new SelectListItem
        //        {
        //            Text = x.ClientName,
        //            Value = x.Id.ToString()
        //        }),
        //    };
        //}

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

                //Members = members.Select(x => new SelectListItem
                //{
                //    Text = $"{x.FirstName} {x.LastName}",
                //    Value = x.Id.ToString()
                //})
            },
            //EditProjectModel = editProjectModel,

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

                //Members = members.Select(x => new SelectListItem
                //{
                //    Text = $"{x.FirstName} {x.LastName}",
                //    Value = x.Id.ToString()
                //})
            },
            Projects = projects,
            PreselectedMembers = preselectedMembers
        };

        return View(viewModel);
    }
    #endregion
}
