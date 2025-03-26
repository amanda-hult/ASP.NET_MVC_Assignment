using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.ViewModels;

namespace Presentation.Controllers;


//[Authorize]
[Route("/projects")]
public class ProjectsController : Controller
{
    private readonly IWebHostEnvironment _env;

    public ProjectsController(IWebHostEnvironment env)
    {
        _env = env;
    }

    [HttpGet]
    [Route("/projects")]
    public IActionResult Projects()
    {
        var viewModel = new ProjectViewModel();
        return View(viewModel);
    }


    [HttpPost]
    public IActionResult AddProject(AddProjectModel model)
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

        //if (model.ProjectImage == null || model.ProjectImage.Length == 0)
        //{
        //    filePath = "/images/projectimage-standard.svg";
        //}
        //else
        //{
        //    var uploadFolder = Path.Combine(_env.WebRootPath, "uploads");
        //    Directory.CreateDirectory(uploadFolder);

        //    filePath = Path.Combine(uploadFolder, $"{Guid.NewGuid()}_{Path.GetFileName(model.ProjectImage.FileName)}");

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await model.ProjectImage.CopyToAsync(stream);
        //    }
        //}

        // send to projectService
        return RedirectToAction("Projects", "Projects");
    }
}



