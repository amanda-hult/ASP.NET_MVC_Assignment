using System.Security.Claims;
using Business.Interfaces;
using Business.Models.Notifications;
using Business.Models.Users;
using Business.Services;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Presentation.Hubs;
using Presentation.Models;

namespace Presentation.Controllers;

public class MemberController : Controller
{
    private readonly IWebHostEnvironment _env;
    private readonly IUserService _userService;
    private readonly UserManager<UserEntity> _userManager;
    private readonly INotificationService _notificationService;
    private readonly IHubContext<NotificationHub> _hubContext;

    public MemberController(IWebHostEnvironment env, IUserService userService, UserManager<UserEntity> userManager, INotificationService notificationService, IHubContext<NotificationHub> hubContext)
    {
        _env = env;
        _userService = userService;
        _userManager = userManager;
        _notificationService = notificationService;
        _hubContext = hubContext;
    }

    #region Add Member
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

        if (result)
        {
            var member = await _userManager.FindByEmailAsync(model.Email);

            if (member != null)
            {
                var notificationCreateModel = new NotificationCreateModel
                {
                    Message = $"{member.FirstName} {member.LastName} was added.",
                    NotificationTypeId = 1
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

    #endregion

    #region Edit Member
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
