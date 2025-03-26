using Business.Interfaces;
using Business.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.Models;

namespace Presentation.ViewModels;

public class ProjectViewModel
{
    private readonly IUserService _userService;
    private readonly IStatusService _statusService;

    public ProjectViewModel(UserService userService, StatusService statusService)
    {
        _userService = userService;
        _statusService = statusService;
    }


    //    public List<SelectListItem> Members { get; set; } = new();




    //    public async Task PopulateMemberList()
    //    {
    //        var members = await _userService.GetAllUsersAsync();
    //        Members = members.Select(x => new SelectListItem
    //        {
    //            Value = x.Name,
    //            Text = x.Description,
    //        }).ToListAsync();
    //    }


    public string Title { get; set; } = "Projects";
    public AddProjectModel AddProjectModel { get; set; } = new AddProjectModel();
    public EditProjectModel EditProjectModel { get; set; } = new EditProjectModel();


    public IEnumerable<SelectListItem> Statuses { get; set; } = new List<SelectListItem>();

    public async Task PopulateStatusList()
    {
        var statuses = await _statusService.GetAllStatuses();
        Statuses = statuses.Select(x => new SelectListItem
        {
            Text = x.StatusName,
            Value = x.StatusId.ToString(),
        });
    }
}
