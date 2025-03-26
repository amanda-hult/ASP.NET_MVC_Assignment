using Business.Models;
using Presentation.Models;

namespace Presentation.ViewModels;

public class MembersViewModel
{
    public IEnumerable<UserModel> Members { get; set; } = new List<UserModel>();
    public AddMemberModel AddMemberModal { get; set; } = new AddMemberModel();
    public EditMemberModel EditMemberModal { get; set; } = new EditMemberModel();
}
