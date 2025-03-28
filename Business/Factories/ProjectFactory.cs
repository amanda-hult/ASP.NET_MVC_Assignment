using Business.Models;
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
}
