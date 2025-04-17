using Business.Models.Projects;
using Business.Models.Users;
using Presentation.Models;

namespace Presentation.ViewModels;

public class ProjectViewModel
{
    public string Title { get; set; } = "Projects";
    public IEnumerable<ProjectModel> Projects { get; set; } = new List<ProjectModel>();
    public AddProjectModel AddProjectModel { get; set; } = new AddProjectModel();
    //public EditProjectModel EditProjectModel { get; set; } = new EditProjectModel();

    public Dictionary<int, EditProjectModel> EditProjectModel { get; set; } = new();

    public Dictionary<int, List<BasicUserModel>> PreselectedMembers { get; set; } = new();
}
