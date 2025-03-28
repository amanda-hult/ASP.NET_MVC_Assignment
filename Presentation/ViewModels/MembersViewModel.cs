using Business.Models;
using Presentation.Models;

namespace Presentation.ViewModels;

public class MembersViewModel
{
    public string Title { get; set; } = "Team Members";
    public AddMemberModel AddMemberModal { get; set; } = new AddMemberModel();
    public EditMemberModel EditMemberModal { get; set; } = new EditMemberModel();
    public IEnumerable<UserModel> Members { get; set; } = new List<UserModel>();
}
