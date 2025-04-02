using Business.Interfaces;
using Business.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;

public class MemberController : Controller
{
    private readonly IWebHostEnvironment _env;
    private readonly IUserService _userService;

    public MemberController(IWebHostEnvironment env, IUserService userService)
    {
        _env = env;
        _userService = userService;
    }


    [HttpPost]
    public async Task<IActionResult> AddMember(AddMemberModel model)
    {
        var isValidDate = DateTime.TryParse($"{model.SelectedYear}-{model.SelectedMonth}-{model.SelectedDay}", out var dateOfBirth);
        if (!isValidDate)
        {
            ModelState.AddModelError("SelectedDay", "Invalid date");
        }

        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                );

            return BadRequest(new { success = false, errors });
        }

        var userCreateModel = new UserCreateModel
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Phone = model.Phone,
            JobTitle = model.JobTitle,
            DateOfBirth = isValidDate ? dateOfBirth : null,
            Address = model.Address,
            Password = model.Password,
        };

        var result = await _userService.CreateUserAsync(userCreateModel);

        if (!result)
            return StatusCode(500);

        //string filePath;

        //if (model.ProfileImage == null || model.ProfileImage.Length == 0)
        //{
        //    filePath = "/images/avatar-standard.svg";
        //}
        //else
        //{
        //    var uploadFolder = Path.Combine(_env.WebRootPath, "uploads");
        //    Directory.CreateDirectory(uploadFolder);

        //    filePath = Path.Combine(uploadFolder, $"{Guid.NewGuid()}_{Path.GetFileName(model.ProfileImage.FileName)}");

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await model.ProfileImage.CopyToAsync(stream);
        //    }
        //}

        // send to service
        return RedirectToAction("Members", "Admin");
    }



    [HttpPost]
    public IActionResult EditMember(EditMemberModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                );

            return BadRequest(new { success = false, errors });
        }

        //send data to clientservice
        return RedirectToAction("Members", "Admin");
    }
}
