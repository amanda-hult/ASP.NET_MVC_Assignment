using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.Models;
using Presentation.ViewModels;

namespace Presentation.Controllers;


//[Authorize]
[Route("/projects")]
public class ProjectsController : Controller
{
    private readonly IWebHostEnvironment _env;
    private readonly IStatusService _statusService;
    private readonly IClientService _clientService;
    private readonly IUserService _userService;

    public ProjectsController(IWebHostEnvironment env, IStatusService statusService, IClientService clientService, IUserService userService)
    {
        _env = env;
        _statusService = statusService;
        _clientService = clientService;
        _userService = userService;
    }

    [HttpGet]
    [Route("/projects")]
    public async Task<IActionResult> Projects()
    {
        var statuses = await _statusService.GetAllStatuses();
        var clients = await _clientService.GetAllClientsAsync();
        var members = await _userService.GetAllUsersAsync();

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
            }
        };

        return View(viewModel);
    }



    [HttpPost]
    public async Task<IActionResult> AddProject(AddProjectModel model)
    {
        //var statuses = await _statusService.GetAllStatuses();

        //model.Statuses = statuses.Select(x => new SelectListItem
        //{
        //    Text = x.StatusName,
        //    Value = x.StatusId.ToString(),
        //}).ToList();

        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                );

            return BadRequest(new { success = false, errors });
        }
        var status = await _statusService.GetStatusAsync(model.SelectedStatusId);
        var client = await _clientService.GetClientAsync(model.SelectedClientId);
        var members = await _userService.GetUsersByIdAsync(model.SelectedMemberId);

        var projectCreateModel = new ProjectCreateModel
        {
            ProjectImage = model.ProjectImage,
            ProjectName = model.ProjectName,
            Description = model.Description,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Budget = model.Budget,
            Client = client,
            Users = members.ToList(),
            Status = status,
        };

        //string filePath;

        //if (model.ProjectImage == null || model.ProjectImage.Length == 0)
        //{
        //    filePath = "/images/projectimage-standard.svg";
        //}
        //else
        //{
        //    var uploadFolder = Path.Combine(_env.WebRootPath, "uploads");
        //    Directory.CreateDirectory(uploadFolder);

        //    filePath = Path.Combine(uploadFolder, $"{Guid.NewGuid()}_{Path.GetFileName(model.ProjectImage.FileName)}");

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await model.ProjectImage.CopyToAsync(stream);
        //    }
        //}

        // send to projectService
        return RedirectToAction("Projects", "Projects");
    }
}



