using System.Linq.Expressions;
using Data.Entities;

namespace Data.Interfaces;

public interface IProjectRepository : IBaseRepository<ProjectEntity>
{
    Task<bool> UserExistsInProject(string userId);
    Task<IEnumerable<ProjectEntity>> GetProjectsByStringAsync(string term);
    Task<ProjectEntity> UpdateProjectAsync(Expression<Func<ProjectEntity, bool>> predicate, ProjectEntity updatedEntity);
}
