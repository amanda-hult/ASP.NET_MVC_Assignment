using System.Security.Claims;
using Business.Interfaces;
using Business.Models.Notifications;
using Business.Models.Users;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentation.Handlers;
using Presentation.Hubs;
using Presentation.Models;

namespace Presentation.Controllers;

public class MemberController : Controller
{
    private readonly IUserService _userService;
    private readonly UserManager<UserEntity> _userManager;
    private readonly INotificationService _notificationService;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly IFileHandler _fileHandler;

    public MemberController(IUserService userService, UserManager<UserEntity> userManager, INotificationService notificationService, IHubContext<NotificationHub> hubContext, IFileHandler fileHandler)
    {
        _userService = userService;
        _userManager = userManager;
        _notificationService = notificationService;
        _hubContext = hubContext;
        _fileHandler = fileHandler;
    }

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
            {
                var notificationCreateModel = new NotificationCreateModel
                {
                    Message = $"New member: {member.FirstName} {member.LastName} was added.",
                    NotificationTypeId = 1,
                    Image = member.UserImageUrl
                };

                await _notificationService.AddNotificationAsync(notificationCreateModel);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var notifications = await _notificationService.GetNotificationsAsync(userId);
                var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();
                if (newNotification != null)
                {
                    await _hubContext.Clients.All.SendAsync("AdminRecieveNotification", newNotification);
                }
            }
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
            imageUri = "/images/avatar-standard.svg";
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
            Address = model.Address,
            Password = model.Password,
        };

        var result = await _userService.UpdateUserAsync(userEditModel);

        // if created, send notifications to Admin roles
        if (result == 201)
        {
            var member = await _userManager.FindByEmailAsync(model.Email);

            if (member != null)
            {
                var notificationCreateModel = new NotificationCreateModel
                {
                    Message = $"Member {member.FirstName} {member.LastName} was updated.",
                    NotificationTypeId = 1,
                    Image = member.UserImageUrl
                };

                await _notificationService.AddNotificationAsync(notificationCreateModel);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var notifications = await _notificationService.GetNotificationsAsync(userId);
                var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();
                if (newNotification != null)
                {
                    await _hubContext.Clients.All.SendAsync("AdminRecieveNotification", newNotification);
                }
            }
        }

        if (result == 409)
            return Conflict(new { message = "Member with the same email address already exists." });

        if (result == 500)
            return StatusCode(500, new { message = "Unexpected error." });


        return RedirectToAction("Members", "Admin");
    }
    #endregion

    #region Delete Member
    //[HttpPost]
    //public async Task<IActionResult> DeleteMember(string id)
    //{
    //    var result = await _userService.DeleteUserAsync(id);

    //    if (result == 201)
    //        return RedirectToAction("Members", "Admin");

    //    if (result == 404)
    //        return NotFound(new { message = "Member not found." });

    //    if (result == 409)
    //        return Conflict(new { message = "Member cannot be deleted, since it exists in a project." });

    //    return StatusCode(500, new { message = "Unexpected error." });

    //}
    #endregion

    #region Search Members
    [HttpGet]
    public async Task<JsonResult> SearchMember(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return Json(new List<object>());

        var members = await _userService.GetBasicUsersByStringAsync(term);

        return Json(members);
    }
    #endregion
}