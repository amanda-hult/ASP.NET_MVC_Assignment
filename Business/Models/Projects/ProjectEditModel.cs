using Business.Models.Clients;
using Business.Models.Users;

namespace Business.Models.Projects;

public class ProjectEditModel
{
    public int Id { get; set; }
    public string? ProjectImage { get; set; }

    public string ProjectName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public decimal? Budget { get; set; }


    public int? ClientId { get; set; }
    public ClientModel Client { get; set; } = null!;


    public List<ProjectUserModel> ProjectUsers { get; set; } = new List<ProjectUserModel>();

    public int? StatusId { get; set; }
    public StatusModel Status { get; set; } = null!;

    //public int? SelectedStatusId { get; set; }

    //public IEnumerable<SelectListItem> Statuses { get; set; } = new List<SelectListItem>();
}
