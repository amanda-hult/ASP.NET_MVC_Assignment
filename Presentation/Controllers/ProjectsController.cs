using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;
using Business.Interfaces;
using Business.Models.Clients;
using Business.Models.Notifications;
using Business.Models.Projects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentation.Handlers;
using Presentation.Hubs;
using Presentation.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Presentation.Controllers;

//[Authorize]
public class ProjectsController : Controller
{
    private readonly IStatusService _statusService;
    private readonly IClientService _clientService;
    private readonly IUserService _userService;
    private readonly IProjectService _projectService;
    private readonly INotificationService _notificationService;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly IFileHandler _fileHandler;

    public ProjectsController(IStatusService statusService, IClientService clientService, IUserService userService, IProjectService projectService, INotificationService notificationService, IHubContext<NotificationHub> hubContext, IFileHandler fileHandler)
    {
        _statusService = statusService;
        _clientService = clientService;
        _userService = userService;
        _projectService = projectService;
        _notificationService = notificationService;
        _hubContext = hubContext;
        _fileHandler = fileHandler;
    }

    #region Add Project
    [HttpPost]
    public async Task<IActionResult> AddProject(AddProjectModel model, string SelectedMemberIdsAdd)
    {
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

        string imageUri;

        if (model.ProjectImage == null ||  model.ProjectImage.Length == 0)
        {
            imageUri = "/images/project-image-standard.svg";
        }
        else
        {
            imageUri = await _fileHandler.UploadFileAsync(model.ProjectImage);
        }

        //if (imageUri == null || model.ProjectImage == null || model.ProjectImage.Length == 0)
        //{
            //string filePath;
            //imageUri = "/images/project-image-standard.svg";

            //if (model.ProjectImage == null || model.ProjectImage.Length == 0)
            //{
            //    filePath = "/images/project-image-standard.svg";
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
        //}


        var status = await _statusService.GetStatusAsync(model.SelectedStatusId);
        var client = await _clientService.GetClientAsync(model.SelectedClientId);
        //var members = await _userService.GetUsersByIdAsync(model.SelectedMemberIds);


        var memberIds = JsonSerializer.Deserialize<List<string>>(SelectedMemberIdsAdd) ?? new();
        var projectUsers = memberIds.Where(id => !string.IsNullOrWhiteSpace(id)).Select(id => new ProjectUserModel
        {
            UserId = id,
        }).ToList();


        var projectCreateModel = new ProjectCreateModel
        {
            ProjectImage = imageUri,
            ProjectName = model.ProjectName,
            Description = model.Description,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Budget = model.Budget,
            Client = client,
            ProjectUsers = projectUsers,
            Status = status,
        };

        var (succeeded, statuscode, projectId) = await _projectService.CreateProjectAsync(projectCreateModel);
        if (succeeded)
        {
            var project = await _projectService.GetProjectAsync(projectId);

            if (project != null)
            {
                var notificationCreateModel = new NotificationCreateModel
                {
                    Message = $"{project.ProjectName} was added.",
                    NotificationTypeId = 2,
                    Image = imageUri,                    
                };

                await _notificationService.AddNotificationAsync(notificationCreateModel);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var notifications = await _notificationService.GetNotificationsAsync(userId);
                var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();
                if (newNotification != null)
                {
                    await _hubContext.Clients.All.SendAsync("RecieveNotification", newNotification);
                }
            }
        }
        if (statuscode == 500)
        {
            return StatusCode(500);
        }

        if (statuscode == 409)
        {
            return StatusCode(409);
        }

        return RedirectToAction("Projects", "Admin");
    }
    #endregion

    #region Edit Project
    [HttpPost]
    public async Task<IActionResult> EditProject(EditProjectModel model)
    {
        var memberIds = string.IsNullOrWhiteSpace(model.SelectedMemberIdsEdit)
            ? []
            : JsonSerializer.Deserialize<List<string>>(model.SelectedMemberIdsEdit);

        if (memberIds.Count == 0)
        {
            ModelState.AddModelError("SelectedMemberIdsEdit", "Please select at least one member");
        }

        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                );
            return BadRequest(new { success = false, errors });
        }


        var projectUsers = memberIds.Where(id => !string.IsNullOrWhiteSpace(id)).Select(id => new ProjectUserModel
        {
            UserId = id,
        }).ToList();

        string imageUri;
        if (model.ProjectImage == null || model.ProjectImage.Length == 0)
        {
            imageUri = "/images/project-image-standard.svg";
        }
        else
        {
            imageUri = await _fileHandler.UploadFileAsync(model.ProjectImage);
        }

        var projectEditModel = new ProjectEditModel
        {
            Id = model.Id,
            ProjectImage = imageUri,
            ProjectName = model.ProjectName,
            Description = model.Description,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Budget = model.Budget,
            ClientId = model.SelectedClientId,
            StatusId = model.SelectedStatusId,
            ProjectUsers = projectUsers
        };

        var result = await _projectService.UpdateProjectAsync(projectEditModel);

        if (result == 200)
        {

            var project = await _projectService.GetProjectAsync(model.Id);
            if (project != null)
            {
                var notificationCreateModel = new NotificationCreateModel
                {
                    Message = $"Project {project.ProjectName} was updated.",
                    NotificationTypeId = 2,
                    Image = project.ProjectImageUrl ?? "/images/project-image-standard.svg"
                };

                await _notificationService.AddNotificationAsync(notificationCreateModel);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var notifications = await _notificationService.GetNotificationsAsync(userId);
                var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();
                if (newNotification != null)
                {
                    await _hubContext.Clients.All.SendAsync("RecieveNotification", newNotification);
                }
            }

            return RedirectToAction("Projects", "Admin");
        }

        if (result == 409)
            return Conflict(new { message = "Project with the same name and client alredy exists." });

        if (result == 404)
            return NotFound(new { message = "Project not found" });


        return StatusCode(500, new { message = "unexpected error." });

        // fix errormessages
        //ViewBag.ErrorMessage = "Could not update the project.";
        //return View(model);

    }

    #endregion

    #region Add Member to Project
    public async Task<IActionResult> AddMemberToProject(int Id, string SelectedMemberIdsUpdateMembers)
    {
        var memberIds = string.IsNullOrWhiteSpace(SelectedMemberIdsUpdateMembers)
                    ? []
                    : JsonSerializer.Deserialize<List<string>>(SelectedMemberIdsUpdateMembers);

        if (memberIds.Count == 0)
        {
            ModelState.AddModelError("SelectedMemberIdsEdit", "Please select at least one member");
            return BadRequest(new { success = false, errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                )
            });
        }

        var result = await _projectService.AddUserToProjectAsync(Id, memberIds);

        if (result == 200)
        {

            var project = await _projectService.GetProjectAsync(Id);
            if (project != null)
            {
                var notificationCreateModel = new NotificationCreateModel
                {
                    Message = $"Project {project.ProjectName} was updated.",
                    NotificationTypeId = 2,
                    Image = project.ProjectImageUrl ?? "/images/project-image-standard.svg"
                };

                await _notificationService.AddNotificationAsync(notificationCreateModel);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var notifications = await _notificationService.GetNotificationsAsync(userId);
                var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();
                if (newNotification != null)
                {
                    await _hubContext.Clients.All.SendAsync("RecieveNotification", newNotification);
                }
            }

            return RedirectToAction("Projects", "Admin");
        }


        if (result == 404)
            return NotFound(new { message = "Project not found" });


        return StatusCode(500, new { message = "unexpected error." });
    }
    #endregion


    #region Delete Project
    public async Task<IActionResult> DeleteProject(int id)
    {
        var result = await _projectService.DeleteProjectAsync(id);

        if (result == 204)
        {

            var project = await _projectService.GetProjectAsync(id);
            if (project != null)
            {
                var notificationCreateModel = new NotificationCreateModel
                {
                    Message = $"Project {project.ProjectName} was deleted.",
                    NotificationTypeId = 2
                };

                await _notificationService.AddNotificationAsync(notificationCreateModel);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var notifications = await _notificationService.GetNotificationsAsync(userId);
                var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();
                if (newNotification != null)
                {
                    await _hubContext.Clients.All.SendAsync("RecieveNotification", newNotification);
                }
            }

            return RedirectToAction("Projects", "Admin");
        }

        if (result == 404)
            return NotFound(new { message = "Project not found." });

        return StatusCode(500, new { message = "Unexpected error." });
    }
    #endregion
}



