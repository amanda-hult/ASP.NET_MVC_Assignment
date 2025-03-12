using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.ViewModels;

namespace Presentation.Controllers;

[Route("projects")]
public class ProjectsController : Controller
{
    private readonly IWebHostEnvironment _env;

    public ProjectsController(IWebHostEnvironment env)
    {
        _env = env;
    }

    [Route("")]
    public IActionResult Projects()
    {
        return View();
    }


    [HttpGet]
    [Route("/addproject")]
    public IActionResult AddProject()
    {
        var viewModel = new AddProjectViewModel();
        return View(viewModel);
    }


    [HttpPost]
    [Route("/addproject")]
    public async Task<IActionResult> AddProject([Bind(Prefix = "AddProjectModel")] AddProjectModel model)
    {
        if (!ModelState.IsValid)
        {
            AddProjectViewModel viewModel = new()
            {
                AddProjectModel = model
            };
            return View(viewModel);
        }

        string filePath;

        if (model.ProjectImage == null ||  model.ProjectImage.Length == 0)
        {
            filePath = "/images/projectimage-standard.svg";
        }
        else
        {
            var uploadFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadFolder);

            filePath = Path.Combine(uploadFolder, $"{Guid.NewGuid()}_{Path.GetFileName(model.ProjectImage.FileName)}");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ProjectImage.CopyToAsync(stream);
            }
        }
        return View();
    }
}



