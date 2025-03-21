using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.ViewModels;

namespace Presentation.Controllers;

public class MemberController : Controller
{
    private readonly IWebHostEnvironment _env;

    public MemberController(IWebHostEnvironment env)
    {
        _env = env;
    }


    [HttpPost]
    public async Task<IActionResult> AddMember(AddMemberModel model)
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
}
