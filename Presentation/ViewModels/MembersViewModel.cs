using Business.Models.Users;
using Presentation.Models;

namespace Presentation.ViewModels;

public class MembersViewModel
{
    public string Title { get; set; } = "Team Members";
    public AddMemberModel AddMemberModel { get; set; } = new AddMemberModel();
    public EditMemberModel EditMemberModel { get; set; } = new EditMemberModel();
    public IEnumerable<UserModel> Members { get; set; } = new List<UserModel>();
}
