using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.ViewModels;

namespace Presentation.Controllers;

[Route("/admin")]
public class AdminController : Controller
{
    private readonly IWebHostEnvironment _env;

    public AdminController(IWebHostEnvironment env)
    {
        _env = env;
    }


    [HttpGet]
    [Route("/members")]
    public IActionResult Members()
    {
        var viewModel = new MembersViewModel();
        return View(viewModel);
    }





    [HttpGet]
    [Route("/addmember")]
    public IActionResult AddMember()
    {
        var viewModel = new AddMemberViewModel();
        return View(viewModel);
    }


    [HttpPost]
    [Route("/addmember")]
    public async Task<IActionResult> AddMember([Bind(Prefix = "AddMemberModel")] AddMemberModel model)
    {
        if (!ModelState.IsValid)
        {
            AddMemberViewModel viewModel = new()
            {
                AddMemberModel = model
            };
            return View(viewModel);
        }

        string filePath;

        if (model.ProfileImage == null || model.ProfileImage.Length == 0)
        {
            filePath = "/images/avatar-standard.svg";
        }
        else
        {
            var uploadFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadFolder);

            filePath = Path.Combine(uploadFolder, $"{Guid.NewGuid()}_{Path.GetFileName(model.ProfileImage.FileName)}");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ProfileImage.CopyToAsync(stream);
            }
        }

        return View();
    }
}
