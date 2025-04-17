using Business.Models.Projects;

namespace Business.Interfaces;

public interface IProjectService
{
    Task<(bool succeded, int statuscode, int? projectId)> CreateProjectAsync(ProjectCreateModel model);
    Task<IEnumerable<ProjectModel>> GetAllProjectsAsync();
    Task<ProjectModel> GetProjectAsync(int? id);
    Task<int> UpdateProjectAsync(ProjectEditModel model);
    Task<int> DeleteProjectAsync(int id);
}
