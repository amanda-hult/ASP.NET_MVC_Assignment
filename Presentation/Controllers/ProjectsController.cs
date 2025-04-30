using System.Text.Json;
using Business.Interfaces;
using Business.Models.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Handlers;
using Presentation.Models;
using Presentation.Services;

namespace Presentation.Controllers;

[Authorize]
public class ProjectsController(IStatusService statusService, IClientService clientService, IProjectService projectService, IFileHandler fileHandler, HelperService helperService) : Controller
{
    private readonly IStatusService _statusService = statusService;
    private readonly IClientService _clientService = clientService;
    private readonly IProjectService _projectService = projectService;
    private readonly IFileHandler _fileHandler = fileHandler;
    private readonly HelperService _helperService = helperService;

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

        var status = await _statusService.GetStatusAsync(model.SelectedStatusId);
        var client = await _clientService.GetClientAsync(model.SelectedClientId);

        var memberIds = string.IsNullOrWhiteSpace(SelectedMemberIdsAdd)
            ? []
            : JsonSerializer.Deserialize<List<string>>(SelectedMemberIdsAdd);

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
            await _helperService.HandleNotifications(projectId.ToString(), 2, imageUri, "Add", true);


        if (statuscode == 500)
            return StatusCode(500);

        if (statuscode == 409)
            return StatusCode(409);

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
            if (!string.IsNullOrEmpty(model.ExistingImage))
            {
                imageUri = model.ExistingImage;
            }
            else
            {
                imageUri = "/images/project-image-standard.svg";
            }
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
            await _helperService.HandleNotifications(model.Id.ToString(), 2, imageUri, "Edit", true);
            return RedirectToAction("Projects", "Admin");
        }

        if (result == 409)
            return Conflict(new { message = "Project with the same name and client alredy exists." });

        if (result == 404)
            return NotFound(new { message = "Project not found" });


        return StatusCode(500, new { message = "unexpected error." });
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
                return RedirectToAction("Projects", "Admin");
        }

        if (result == 404)
            return NotFound(new { message = "Project not found" });


        return StatusCode(500, new { message = "An unexpected error occured." });
    }
    #endregion

    #region Delete Project
    public async Task<IActionResult> DeleteProject(int id)
    {
        var result = await _projectService.DeleteProjectAsync(id);

        if (result == 204)
            return RedirectToAction("Projects", "Admin");

        if (result == 404)
            return NotFound(new { message = "Project not found." });

        return StatusCode(500, new { message = "Unexpected error." });
    }
    #endregion
}



