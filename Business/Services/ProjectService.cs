using System.Diagnostics;
using Business.Factories;
using Business.Interfaces;
using Business.Models.Projects;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IClientService _clientService;
    private readonly IUserService _userService;
    private readonly IStatusService _statusService;

    public ProjectService(IProjectRepository projectRepository, IClientService clientService, IUserService userService, IStatusService statusService)
    {
        _projectRepository = projectRepository;
        _clientService = clientService;
        _userService = userService;
        _statusService = statusService;
    }

    // CREATE
    public async Task<(bool succeded, int statuscode, int? projectId)> CreateProjectAsync(ProjectCreateModel model)
    {
        // check if project with same project name and client exists
        bool exists = await _projectRepository.ExistsAsync(x => x.ProjectName == model.ProjectName && x.ClientId == model.Client.Id);
        if (exists)
            return (false, 409, null);

        try
        {
            // get client, status and user entities
            var clientEntity = await _clientService.GetClientEntityAsync(model.Client.Id);
            var statusEntity = await _statusService.GetStatusEntityAsync(model.Status.StatusId);

            var userIds = model.Users.Select(x => x.Id).ToList();
            var users = await _userService.GetUserEntitiesByIdAsync(userIds);

            await _projectRepository.BeginTransactionAsync();

            var projectEntity = ProjectFactory.Create(model, clientEntity, statusEntity, users);
            var createdProject = await _projectRepository.CreateAsync(projectEntity);
            if (createdProject == null)
            {
                await _projectRepository.RollbackTransactionAsync();
                return (false, 500, null);
            }

            await _projectRepository.SaveAsync();
            await _projectRepository.CommitTransactionAsync();
            return (true, 201, createdProject.ProjectId);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating project: {ex.Message}");
            await _projectRepository.RollbackTransactionAsync();
            return (false, 500, null);
        }
    }

    public async Task<IEnumerable<ProjectModel>> GetAllProjectsAsync()
    {
        var list = await _projectRepository.GetAllAsync(query =>
                query
                    .Include(p => p.Client)
                    .Include(p => p.Status)
                    .Include(p => p.ProjectUsers)
                        .ThenInclude(pu => pu.User)
                );
        if (list == null)
            return null!;

        var projects = list.Select(ProjectFactory.Create).ToList();
        return projects;
    }

    public async Task<ProjectModel> GetProjectAsync(int? id)
    {
        var projectEntity = await _projectRepository.GetAsync(x => x.ProjectId == id);
        if (projectEntity == null)
            return null!;

        var projectModel = ProjectFactory.CreateBasic(projectEntity);
        return projectModel;
    }
}
