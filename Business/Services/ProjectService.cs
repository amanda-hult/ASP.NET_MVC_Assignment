using System.Diagnostics;
using Business.Factories;
using Business.Interfaces;
using Business.Models.Projects;
using Data.Entities;
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

    #region Create
    public async Task<(bool succeded, int statuscode, int? projectId)> CreateProjectAsync(ProjectCreateModel model)
    {
        // check if project with same project name and client exists
        bool exists = await _projectRepository.ExistsAsync(x => x.ProjectName == model.ProjectName && x.ClientId == model.Client.Id);
        if (exists)
            return (false, 409, null);

        try
        {
            // get client and status entities
            var clientEntity = await _clientService.GetClientEntityAsync(model.Client.Id);
            if (clientEntity == null)
                return (false, 404, null);

            var statusEntity = await _statusService.GetStatusEntityAsync(model.Status.StatusId);
            if (statusEntity == null)
                return (false, 404, null);

            var userIds = model.ProjectUsers.Select(x => x.UserId).ToList();

            await _projectRepository.BeginTransactionAsync();

            var projectEntity = ProjectFactory.Create(model, clientEntity, statusEntity);
            var createdProject = await _projectRepository.CreateAsync(projectEntity);
            if (createdProject == null)
            {
                await _projectRepository.RollbackTransactionAsync();
                return (false, 500, null);
            }

            if (userIds.Count != 0)
            {
                foreach (var id in userIds)
                {
                    createdProject.ProjectUsers.Add(new ProjectUserEntity
                    {
                        ProjectId = createdProject.ProjectId,
                        UserId = id
                    });
                }
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
    #endregion

    #region Read
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
    #endregion

    #region Update
    public async Task<int> UpdateProjectAsync(ProjectEditModel model)
    {
        await _projectRepository.BeginTransactionAsync();

        try
        {
            // get project
            var existingProject = await _projectRepository.GetAsync(
                x => x.ProjectId == model.Id,
                q => q.Include(p => p.Client)
                      .Include(p => p.Status)
                      .Include(p => p.ProjectUsers)
                            .ThenInclude(pu => pu.User)
            );

            if (existingProject == null)
                return 404;

            // check if project with the same projectname and client id exists
            var projectNameLowercase = model.ProjectName.ToLower();
            bool projectExists = await _projectRepository.ExistsAsync(x => x.ProjectId != model.Id && x.ProjectName.ToLower() == projectNameLowercase && x.ClientId == model.ClientId);
            if (projectExists)
            {
                return 409;
            }

            // get client
            var clientEntity = await _clientService.GetClientEntityAsync(model.ClientId);
            if (clientEntity == null)
                return 404;

            // get status
            var statusEntity = await _statusService.GetStatusEntityAsync(model.StatusId);
            if (statusEntity == null)
                return 404;

            // get and replace members
            var existingMembers = existingProject.ProjectUsers.ToList();
            var newMemberIds = model.ProjectUsers.Select(pu => pu.UserId).ToList();

            var membersToRemove = existingMembers.Where(pu => !newMemberIds.Contains(pu.UserId)).ToList();
            foreach (var member in membersToRemove)
            {
                existingProject.ProjectUsers.Remove(member);
            }

            foreach (var id in newMemberIds)
            {
                bool alreadyExists = existingProject.ProjectUsers.Any(pu => pu.UserId == id);

                if (!alreadyExists)
                {
                    existingProject.ProjectUsers.Add(new ProjectUserEntity
                    {
                        ProjectId = existingProject.ProjectId,
                        UserId = id
                    });
                }
            }


            // update project
            var projectToUpdate = ProjectFactory.Update(model, existingProject, clientEntity, statusEntity);

            var updatedProject = await _projectRepository.UpdateProjectAsync(x => x.ProjectId == model.Id, projectToUpdate);

            await _projectRepository.SaveAsync();
            await _projectRepository.CommitTransactionAsync();

            return 200;
        }
        catch (Exception ex)
        {
            await _projectRepository.RollbackTransactionAsync();
            Debug.WriteLine($"Error in projectService creating project: {ex.Message}");
            return 500;
        }
    }



    public async Task<int> AddUserToProjectAsync(int projectId, List<string> memberIds)
    {
        await _projectRepository.BeginTransactionAsync();
        try
        {
            var existingProject = await _projectRepository.GetAsync(
                x => x.ProjectId == projectId,
                q => q.Include(p => p.ProjectUsers)
                        .ThenInclude(pu => pu.User)
            );

            if (existingProject == null)
                return 404;


            // get and replace members
            var existingMembers = existingProject.ProjectUsers.ToList();

            var membersToRemove = existingMembers.Where(pu => !memberIds.Contains(pu.UserId)).ToList();
            foreach (var member in membersToRemove)
            {
                existingProject.ProjectUsers.Remove(member);
            }

            foreach (var id in memberIds)
            {
                bool alredyExists = existingProject.ProjectUsers.Any(pu => pu.UserId == id);

                if (!alredyExists)
                {
                    existingProject.ProjectUsers.Add(new ProjectUserEntity
                    {
                        ProjectId = existingProject.ProjectId,
                        UserId = id
                    });
                }
            }

            await _projectRepository.UpdateProjectAsync(x => x.ProjectId == projectId, existingProject);
            await _projectRepository.SaveAsync();
            await _projectRepository.CommitTransactionAsync();

            return 200;
        }
        catch (Exception ex)
        {
            await _projectRepository.RollbackTransactionAsync();
            Debug.WriteLine($"Error in projectService adding members to project: { ex.Message}");
            return 500;
        }
    }
    #endregion

    #region Delete
    public async Task<int> DeleteProjectAsync(int id)
    {
        var exists = await _projectRepository.ExistsAsync(x => x.ProjectId == id);
        if (!exists)
            return 404;

        bool result = await _projectRepository.DeleteAsync(x => x.ProjectId == id);
        if (!result)
            return 500;

        return 204;

    }
    #endregion
}
