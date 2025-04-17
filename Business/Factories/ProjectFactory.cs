using Business.Models;
using Business.Models.Clients;
using Business.Models.Projects;
using Business.Models.Users;
using Data.Entities;

namespace Business.Factories;

public static class ProjectFactory
{
    public static ProjectEntity Create(ProjectCreateModel model, ClientEntity client, StatusEntity status)
    {
        return new ProjectEntity
        {
            ProjectImageUrl = model.ProjectImage,
            ProjectName = model.ProjectName,
            Description = model.Description,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Budget = model.Budget,
            Client = client,
            ClientId = client.ClientId,
            Status = status,
            StatusId = status.StatusId
        };
    }


    public static ProjectModel CreateBasic(ProjectEntity entity)
    {
        return new ProjectModel
        {
            ProjectImageUrl = entity.ProjectImageUrl,
            ProjectName = entity.ProjectName,
        };
    }
    public static ProjectModel Create(ProjectEntity entity)
    {
        return new ProjectModel
        {
            ProjectId = entity.ProjectId,
            ProjectImageUrl = entity.ProjectImageUrl,
            ProjectName = entity.ProjectName,
            Description = entity.Description,
            Budget = entity.Budget,

            StartDate = entity.StartDate,
            EndDate = entity.EndDate,

            ClientId = entity.ClientId,
            Client = new ClientModel
            {
                Id = entity.Client.ClientId,
                ClientName = entity.Client.ClientName
            },

            StatusId = entity.StatusId,
            Status = new StatusModel
            {
                StatusId = entity.Status.StatusId,
                StatusName = entity.Status.StatusName,
            },

            ProjectUsers = entity.ProjectUsers.Select(pu => new ProjectUserModel
            {
                ProjectId = pu.ProjectId,
                UserId = pu.UserId,
                User = new UserModel
                {
                    Id = pu.User.Id,
                    UserImageUrl = pu.User.UserImageUrl
                }
            }).ToList()
        };
    }

    public static ProjectEntity Update(ProjectEditModel model, ProjectEntity project, ClientEntity client, StatusEntity status)
    {
        project.ProjectImageUrl = model.ProjectImage;
        project.ProjectName = model.ProjectName;
        project.Description = model.Description;
        project.Budget = model.Budget;
        project.StartDate = model.StartDate;
        project.EndDate = model.EndDate;
        project.ClientId = client.ClientId;
        project.StatusId = status.StatusId;

        return project;
    }
}
