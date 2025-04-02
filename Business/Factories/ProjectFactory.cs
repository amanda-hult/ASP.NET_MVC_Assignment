using Business.Models;
using Business.Models.Clients;
using Business.Models.Projects;
using Business.Models.Users;
using Data.Entities;

namespace Business.Factories;

public static class ProjectFactory
{
    public static ProjectEntity Create(ProjectCreateModel model, ClientEntity client, StatusEntity status, List<UserEntity> users)
    {
        return new ProjectEntity
        {
            //ProjectImageUrl
            ProjectName = model.ProjectName,
            Description = model.Description,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Budget = model.Budget,
            Client = client,
            Status = status,
            ProjectUsers = users.Select(user => new ProjectUserEntity
            {
                UserId = user.Id,
                User = user
            }).ToList()
        };
    }

    public static ProjectModel Create(ProjectEntity entity)
    {
        return new ProjectModel
        {
            //ProjectImageUrl = entity.ProjectImageUrl,
            ProjectName = entity.ProjectName,
            Description = entity.Description,

            //TimeLeft =
            //StartDate = entity.StartDate,
            //EndDate = entity.EndDate,

            ClientId = entity.ClientId,
            Client = new ClientModel
            {
                Id = entity.Client.ClientId,
                ClientName = entity.ProjectName
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
}
