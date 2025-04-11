using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

[Authorize]

public class ProjectsController(IProjectService projectService) : Controller
{
    private readonly IProjectService _projectService = projectService;


    public async Task<IActionResult> Projects()
    {
        var model = new ProjectsViewModel
        {
            Projects = await _projectService.GetProjectsAsync(),
        }

        return View(model);

    }

    [HttpPost]
    public IActionResult Add(AddProjectViewModel model)
    {
        return Json(new { model });
    }
    [HttpPost]
    public IActionResult Update(EditProjectViewModel model)
    {
        return Json(new { model });
    }
    [HttpPost]
    public IActionResult delete(string id)
    {
        return Json(new { id });
    }
}
