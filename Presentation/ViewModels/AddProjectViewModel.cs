using Business.Interfaces;
using Business.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.Models;

namespace Presentation.ViewModels;

public class AddProjectViewModel
{
    //private readonly IUserService _userService;

    //public AddProjectViewModel(UserService userService)
    //{
    //    _userService = userService;
    //}



    public string Title { get; set; } = "Add Project";
    public AddProjectModel AddProjectModel { get; set; } = new AddProjectModel();

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
}
