using Presentation.Models;

namespace Presentation.ViewModels;

public class MembersViewModel
{
    public string Title { get; set; } = "Team Members";
    public AddMemberModel Member { get; set; } = new AddMemberModel();
}
