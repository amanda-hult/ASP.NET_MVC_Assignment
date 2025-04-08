using Business.Models.Projects;
using Presentation.Models;

namespace Presentation.ViewModels;

public class ProjectViewModel
{
    public string Title { get; set; } = "Projects";
    public IEnumerable<ProjectModel> Projects { get; set; } = new List<ProjectModel>();
    public AddProjectModel AddProjectModel { get; set; } = new AddProjectModel();
    public EditProjectModel EditProjectModel { get; set; } = new EditProjectModel();

}
