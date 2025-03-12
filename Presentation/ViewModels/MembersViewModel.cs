using Business.Models;

namespace Presentation.ViewModels;

public class MembersViewModel
{
    public string Title { get; set; } = "Team Members";
    public UserModel User { get; set; }
}
