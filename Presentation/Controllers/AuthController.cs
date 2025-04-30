using System.Diagnostics;
using System.Security.Claims;
using Business.Interfaces;
using Business.Models.Notifications;
using Business.Models.Users;
using Presentation.Models;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentation.Handlers;
using Presentation.Hubs;
using Presentation.Services;
using Presentation.ViewModels;

namespace Presentation.Controllers;

public class AuthController(IAuthService authService, IFileHandler fileHandler, SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, INotificationService notificationService, IHubContext<NotificationHub> hubContext, HelperService helperService) : Controller
{
    private readonly IAuthService _authService = authService;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly INotificationService _notificationService = notificationService;
    private readonly IHubContext<NotificationHub> _hubContext = hubContext;
    private readonly HelperService _helperService = helperService;
    private readonly IFileHandler _fileHandler = fileHandler;


    #region Sign Up

    [Route("/signup")]
    [HttpGet]
    public IActionResult SignUp()
    {
        var viewModel = new SignUpViewModel();
        return View(viewModel);
    }

    [Route("/signup")]
    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpViewModel viewModel)
    {
        var model = viewModel.Form;

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

        var userEntity = await _userManager.FindByEmailAsync(model.Email);
        if (userEntity != null)
        {
            ModelState.AddModelError("Form.Email", "Email address is already registrered.");

            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                );

            return BadRequest(new { success = false, errors });
        }

        var result = await _authService.SignUpAsync(model);
        if (result)
            return LocalRedirect("~/");

        return StatusCode(500, new { success = false, message = "An unexpected error occured." });
    }

    #endregion

    #region Sign In

    [HttpPost]
    public IActionResult SignInExternal(string provider, string returnUrl = null!)
    {
        if (string.IsNullOrEmpty(provider))
        {
            ModelState.AddModelError("", "Invalid provider");
            var viewModel = new SignInViewModel();
            ViewBag.ReturnUrl = returnUrl;
            return View("SignIn", viewModel);
        }

        var redirectUrl = Url.Action("SignInExternalCallback", "Auth", new { ReturnUrl = returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    public async Task<IActionResult> SignInExternalCallback(string returnUrl = null!, string remoteError = null!)
    {
        if (string.IsNullOrEmpty(returnUrl))
            returnUrl = Url.Content("~/");

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


    //[Route("/signin")]
    [HttpGet]
    public IActionResult SignIn(string returnUrl = "/projects")
    {
        ViewBag.ErrorMessage = "";
        ViewBag.ReturnUrl = returnUrl;
        var viewModel = new SignInViewModel();
        return View(viewModel);
    }

    //[Route("/signin")]
    [HttpPost]
    public async Task<IActionResult> SignIn(SignInViewModel viewModel, string returnUrl = "/projects")
    {
        ViewBag.ErrorMessage = "";
        ViewBag.ReturnUrl = returnUrl;

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
            return View(viewModel);
        }

        var model = viewModel.Form;
        var result = await _authService.SignInAsync(model);

        if (!result)
        {
            ViewBag.ErrorMessage = "Incorrect email or password.";
            return View(viewModel);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ViewBag.ErrorMessage = "Incorrect email or password.";
            return View(viewModel);
        }

        // handle member profile image
        var imageUri = string.IsNullOrWhiteSpace(user.UserImageUrl)
            ? "/images/avatar-standard.svg"
            : user.UserImageUrl;

        var claims = new List<Claim>
        {
            new("FullName", $"{user.FirstName} {user.LastName}"),
            new("ProfileImageUrl", imageUri)
        };

        await _userManager.AddClaimsAsync(user, claims);
        await _signInManager.RefreshSignInAsync(user);
        await _helperService.HandleNotifications(user.Id, 1, imageUri, "SignIn", true);

        return LocalRedirect(returnUrl);
    }

    [HttpGet]
    public IActionResult SignInAdmin(string returnUrl = "/projects")
    {
        ViewBag.ErrorMessage = "";
        ViewBag.ReturnUrl = returnUrl;
        var viewModel = new SignInViewModel();
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> SignInAdmin(SignInViewModel viewModel, string returnUrl = "/projects")
    {
        ViewBag.ErrorMessage = "";
        ViewBag.ReturnUrl = returnUrl;

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
            return View(viewModel);
        }
        var model = viewModel.Form;

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ViewBag.ErrorMessage("Unable to sign in, please try again.");
            return View(viewModel);
        }

        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
        if (!isAdmin)
        {
            ViewBag.ErrorMessage("You need to be an administator to sign in.");
            return View(viewModel);
        }

        var result = await _authService.SignInAsync(model);

        if (result)
        {
            // handle member profile image
            var imageUri = string.IsNullOrWhiteSpace(user.UserImageUrl)
                ? "/images/avatar-standard.svg"
                : user.UserImageUrl;

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new("FullName", $"{user.FirstName} {user.LastName}"),
                    new("ProfileImageUrl", imageUri)
                };

                await _userManager.AddClaimsAsync(user, claims);
                await _signInManager.RefreshSignInAsync(user);
                await _helperService.HandleNotifications(user.Id, 1, imageUri, "SignIn", true);
            }
            return LocalRedirect(returnUrl);
        }
        ViewBag.ErrorMessage = "Incorrect email or password.";
        return View(viewModel);
    }
    #endregion

    #region Sign Out
    public new async Task<IActionResult> SignOut()
    {
        await _authService.SignOutAsync();
        return RedirectToAction("SignIn", "Auth");
    }
    #endregion
}
