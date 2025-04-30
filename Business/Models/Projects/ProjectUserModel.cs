using Business.Models.Users;

namespace Business.Models.Projects;

public class ProjectUserModel
{
    public int ProjectId { get; set; }
    public ProjectModel Project { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public UserModel User { get; set; } = null!;
}
