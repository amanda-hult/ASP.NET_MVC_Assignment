using System.Diagnostics;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;

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
    public async Task<int> CreateProjectAsync(ProjectCreateModel model)
    {
        // check if project with same project name and client exists
        bool exists = await _projectRepository.ExistsAsync(x => x.ProjectName == model.ProjectName && x.ClientId == model.Client.Id);
        if (!exists)
            return 409;

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
                return 500;
            }

            await _projectRepository.SaveAsync();
            await _projectRepository.CommitTransactionAsync();
            return 201;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating project: {ex.Message}");
            await _projectRepository.RollbackTransactionAsync();
            return 500;
        }
    }

    //public async Task<IEnumerable<ProjectModel>> GetAllProjectsAsync()
    //{

    //}
}
