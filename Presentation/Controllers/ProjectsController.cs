using System.Security.Claims;
using Business.Interfaces;
using Business.Models.Notifications;
using Business.Models.Projects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentation.Hubs;
using Presentation.Models;

namespace Presentation.Controllers;


//[Authorize]
public class ProjectsController : Controller
{
    private readonly IWebHostEnvironment _env;
    private readonly IStatusService _statusService;
    private readonly IClientService _clientService;
    private readonly IUserService _userService;
    private readonly IProjectService _projectService;
    private readonly INotificationService _notificationService;
    private readonly IHubContext<NotificationHub> _hubContext;

    public ProjectsController(IWebHostEnvironment env, IStatusService statusService, IClientService clientService, IUserService userService, IProjectService projectService, INotificationService notificationService, IHubContext<NotificationHub> hubContext)
    {
        _env = env;
        _statusService = statusService;
        _clientService = clientService;
        _userService = userService;
        _projectService = projectService;
        _notificationService = notificationService;
        _hubContext = hubContext;
    }

    #region Add Project
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

        string filePath;

        if (model.ProjectImage == null || model.ProjectImage.Length == 0)
        {
            filePath = "/images/project-image-standard.svg";
        }
        else
        {
            var uploadFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadFolder);

            filePath = Path.Combine(uploadFolder, $"{Guid.NewGuid()}_{Path.GetFileName(model.ProjectImage.FileName)}");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ProjectImage.CopyToAsync(stream);
            }
        }

        var status = await _statusService.GetStatusAsync(model.SelectedStatusId);
        var client = await _clientService.GetClientAsync(model.SelectedClientId);
        var members = await _userService.GetUsersByIdAsync(model.SelectedMemberIds);

        var projectCreateModel = new ProjectCreateModel
        {
            ProjectImage = filePath,
            ProjectName = model.ProjectName,
            Description = model.Description,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Budget = model.Budget,
            Client = client,
            Users = members.ToList(),
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


}



