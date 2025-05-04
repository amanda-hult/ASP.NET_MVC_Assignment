using Business.Interfaces;
using Business.Models.Users;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Handlers;
using Presentation.Models;
using Presentation.Services;

namespace Presentation.Controllers;

public class MemberController(IUserService userService, UserManager<UserEntity> userManager, IFileHandler fileHandler, HelperService helperService) : Controller
{
    private readonly IUserService _userService = userService;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly IFileHandler _fileHandler = fileHandler;
    private readonly HelperService _helperService = helperService;

    #region Add Member
    [HttpPost]
    public async Task<IActionResult> AddMember(AddMemberModel model)
    {
        // validate form input
        var isValidDate = DateTime.TryParse($"{model.SelectedYear}-{model.SelectedMonth}-{model.SelectedDay}", out var dateOfBirth);

        if (!isValidDate)
            ModelState.AddModelError("SelectedDay", "Invalid date");

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

        // handle member profile image
        string imageUri;

        if (model.ProfileImage == null || model.ProfileImage.Length == 0)
        {
            imageUri = "/images/avatar-standard.svg";
        }
        else
        {
            imageUri = await _fileHandler.UploadFileAsync(model.ProfileImage);
        }

        // create new UserCreateModel and send to service
        var userCreateModel = new UserCreateModel
        {
            ProfileImage = imageUri,
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

        // if created, send notifications to Admin roles
        if (result == 201)
        {
            var member = await _userManager.FindByEmailAsync(model.Email);

            if (member != null)
                await _helperService.HandleNotifications(member.Id, 1, imageUri, "Add", false);
        }
        if (result == 409)
            return Conflict( new { message = "Member with the same email address already exists." });

        if (result == 500)
            return StatusCode(500, new { message = "Unexpected error." });

        return RedirectToAction("Members", "Admin");
    }

    #endregion

    #region Edit Member
    [HttpPost]
    public async Task<IActionResult> EditMember(EditMemberModel model)
    {
        // validate form input
        var isValidDate = DateTime.TryParse($"{model.SelectedYear}-{model.SelectedMonth}-{model.SelectedDay}", out var dateOfBirth);

        if (!isValidDate)
            ModelState.AddModelError("SelectedDay", "Invalid date");

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

        // handle member profile image
        string imageUri;

        if (model.ProfileImage == null || model.ProfileImage.Length == 0)
        {
            if (!string.IsNullOrEmpty(model.ExistingImage))
            {
                imageUri = model.ExistingImage;
            }
            else
            {
                imageUri = "/images/avatar-standard.svg";
            }
        }
        else
        {
            imageUri = await _fileHandler.UploadFileAsync(model.ProfileImage);
        }

        // create new UserEditModel and send to service
        var userEditModel = new UserEditModel
        {
            Id = model.Id,
            ProfileImage = imageUri,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Phone = model.Phone,
            JobTitle = model.JobTitle,
            DateOfBirth = isValidDate ? dateOfBirth : null,
            Address = model.Address != null
            ? new AddressEditModel
            {
                Id = model.Address.Id,
                StreetName = model.Address.StreetName,
                StreetNumber = model.Address.StreetNumber,
                PostalCode = model.Address.PostalCode,
                City = model.Address.City
            }
            : null,
        };

        var result = await _userService.UpdateUserAsync(userEditModel);

        // if created, send notifications to Admin roles
        if (result == 200)
            await _helperService.HandleNotifications(model.Id, 1, imageUri, "Edit", false);

        if (result == 409)
            return Conflict(new { message = "Member with the same email address already exists." });

        if (result == 500)
            return StatusCode(500, new { message = "Unexpected error." });


        return RedirectToAction("Members", "Admin");
    }
    #endregion

    #region Delete Member
    [HttpPost]
    public async Task<IActionResult> DeleteMember(string id)
    {
        var result = await _userService.DeleteUserAsync(id);

        if (result == 204)
            return RedirectToAction("Members", "Admin");

        if (result == 404)
            return NotFound(new { message = "Member not found." });

        if (result == 409)
            return Conflict(new { message = "Member cannot be deleted, since it exists in a project." });

        return StatusCode(500, new { message = "Unexpected error." });

    }
    #endregion
}