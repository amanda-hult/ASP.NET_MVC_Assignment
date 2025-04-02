using System.Diagnostics;
using System.Security.Claims;
using Business.Interfaces;
using Business.Models;
using Business.Models.Notifications;
using Business.Models.Users;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentation.Hubs;
using Presentation.Models;
using Presentation.ViewModels;

namespace Presentation.Controllers;

public class AuthController : Controller
{
    public AuthController(IAuthService authService, SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, INotificationService notificationService, IHubContext<NotificationHub> hubContext)
    {
        _authService = authService;
        _userManager = userManager;
        _signInManager = signInManager;
        _notificationService = notificationService;
        _hubContext = hubContext;
    }

    private readonly IAuthService _authService;
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly UserManager<UserEntity> _userManager;
    private readonly INotificationService _notificationService;
    private readonly IHubContext<NotificationHub> _hubContext;


    [Route("/signup")]
    [HttpGet]
    public IActionResult SignUp()
    {
        ViewBag.ErrorMessage = null;
        var viewModel = new SignUpViewModel();
        return View(viewModel);
    }

    [Route("/signup")]
    [HttpPost]
    public async Task<IActionResult> SignUp([Bind(Prefix = "")] SignUpModel model)
    {
        if (!ModelState.IsValid)
        {
            SignUpViewModel viewModel = new()
            {
                Form = model
            };
            return View(viewModel);
        }

        //_accountService.ExistsAsync(model);
        var result = await _authService.SignUpAsync(model);
        if (result)
            return LocalRedirect("~/");

        ViewBag.ErrorMessage = "";
        return View();
    }



    [HttpPost]
    public IActionResult SignInExternal(string provider, string returnUrl = null!)
    {
        if (string.IsNullOrEmpty(provider))
        {
            ModelState.AddModelError("", "Invalid provider");
            return View("SignIn");
        }

        var redirectUrl = Url.Action("SignInExternalCallback", "Auth", new { ReturnUrl = returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    public async Task<IActionResult> SignInExternalCallback(string returnUrl = null!, string remoteError = null!)
    {
        if (string.IsNullOrEmpty(returnUrl))
        {
            returnUrl = Url.Content("~/");
        }

        if (!string.IsNullOrEmpty(remoteError))
        {
            ModelState.AddModelError("", $"External error: {remoteError}");
            return View("SignIn");
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
            return RedirectToAction("SignIn");

        var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        if (signInResult.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            var externalUserModel = new ExternalUserModel
            {
                FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty,
                LastName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? string.Empty,
                Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                UserName = $"{info.LoginProvider.ToLower()}_{email}"
            };

            var identityResult = await _authService.SignInExternalAsync(externalUserModel, info);

            if (identityResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("SignIn");
        }
    }



    [Route("/signin")]
    [HttpGet]
    public IActionResult SignIn(string returnUrl = "~/")
    {
        ViewBag.ErrorMessage = "";
        ViewBag.ReturnUrl = returnUrl;
        var viewModel = new SignInViewModel();
        return View(viewModel);
    }

    [Route("/signin")]
    [HttpPost]
    public async Task<IActionResult> SignIn(SignInViewModel viewModel, string returnUrl = "~/")
    {
        ViewBag.ErrorMessage = "";

        var model = viewModel.Form;

        //SignInViewModel signInViewModel = new()
        //{
        //    Form = viewModel.Form
        //};

        if (!ModelState.IsValid)
        {
            foreach (var kvp in ModelState)
            {
                var key = kvp.Key;
                var errors = kvp.Value?.Errors;
                foreach (var error in errors!)
                {
                    Debug.WriteLine($"Field: {key}, Error: {error.ErrorMessage}");
                }
            }
            Debug.WriteLine("Modellen är ogiltig.");
        }
        if (ModelState.IsValid)
        {
            var result = await _authService.SignInAsync(model);

            Debug.WriteLine($"Login secceeded: {result}");

            if (result)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    //var notificationEntity = new NotificationEntity
                    //{
                    //    Message = $"{user.FirstName} {user.LastName} signed in.",
                    //    NotificationTypeId = 1
                    //};
                    var notificationCreateModel = new NotificationCreateModel
                    {
                        Message = $"{user.FirstName} {user.LastName} signed in.",
                        NotificationTypeId = 1
                    };

                    await _notificationService.AddNotificationAsync(notificationCreateModel, user.Id);

                    var notifications = await _notificationService.GetNotificationsAsync(user.Id);
                    var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();
                    if (newNotification != null)
                    {
                        await _hubContext.Clients.All.SendAsync("RecieveNotification", newNotification);
                    }
                }
                return LocalRedirect(returnUrl);
            }




        }

        //bool exists = await _authService.Exists(model.Email);
        //if (!exists)
        //{
        //    ViewBag.ErrorMessage = "The email is not registered. Plese create an account to sign in.";
        //    return View(viewModel);
        //}

        ViewBag.ErrorMessage = "Incorrect email or password.";
        return View(viewModel);
    }

    public new async Task<IActionResult> SignOut()
    {
        await _authService.SignOutAsync();
        return RedirectToAction("SignIn", "Auth");
    }

}
