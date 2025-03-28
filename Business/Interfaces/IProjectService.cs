using Business.Models;

namespace Business.Interfaces;

public interface IProjectService
{
    Task<int> CreateProjectAsync(ProjectCreateModel model);
    //Task<IEnumerable<ProjectModel>> GetAllProjectsAsync();
}
