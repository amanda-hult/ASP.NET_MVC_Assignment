using Business.Models.Clients;

namespace Business.Models.Projects;

public class ProjectModel
{
    public int ProjectId { get; set; }
    public string? ProjectImageUrl { get; set; }
    public string ProjectName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal? Budget { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public int? DaysLeft => EndDate.HasValue ? (EndDate.Value.Date - DateTime.Today).Days : null!;
    public int ClientId { get; set; }
    public ClientModel Client { get; set; } = null!;

    public int StatusId { get; set; }
    public StatusModel Status { get; set; } = null!;

    public List<ProjectUserModel> ProjectUsers { get; set; } = new List<ProjectUserModel>();
}
