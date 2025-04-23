using Business.Models.Notifications;
using Business.Models.Projects;

namespace Business.Models.Users;

public class UserModel
{
    public string Id { get; set; } = null!;
    public string? UserImageUrl { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string? JobTitle { get; set; }
    public DateTime? DateOfBirth { get; set; }

    public int? DayOfBirth => DateOfBirth?.Day;
    public int? MonthOfBirth => DateOfBirth?.Month;
    public int? YearOfBirth => DateOfBirth?.Year;

    public AddressModel? Address { get; set; }

    public List<ProjectUserModel> ProjectUsers { get; set; } = new List<ProjectUserModel>();
    public List<NotificationDismissedModel> DismissedNotifications { get; set; } = new List<NotificationDismissedModel>();
}

