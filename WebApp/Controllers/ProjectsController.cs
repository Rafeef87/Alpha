using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Helpers;
using WebApp.Models;

namespace WebApp.Controllers;

[Authorize]
public class ProjectsController(IProjectService projectService, ILogger<ProjectsController> logger) : Controller
{
    private readonly IProjectService _projectService = projectService;
    private readonly ILogger<ProjectsController> _logger = logger;



    [HttpGet]
    [Route("projects")]
    public async Task<IActionResult> Projects()
    {
     
            var projectsResult = await _projectService.GetProjectsAsync();

        if (!projectsResult.Succeeded)
            return StatusCode(500, projectsResult.Error);

        return View(projectsResult.Result);
    }

    [HttpPost]
    [Route("projects")]
    public async Task<IActionResult> Add(AddProjectViewModel model)
    {
        if (!ModelState.IsValid)
            return Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors) });

        // Map the view model to the corresponding form data
        var form = model.MapToAddProjectFormData();
        var result = await _projectService.CreateProjectAsync(form);

        return Json(new { success = result.Succeeded });
    }

    [HttpPost("update")]

    public async Task<IActionResult> Update(EditProjectViewModel model)
    {
        if (!ModelState.IsValid)
            return Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors) });

        // Map the view model to EditProjectFormData using the new extension
        var form = model.MapToEditProjectFormData();
        var result = await _projectService.UpdateProjectAsync(form);

        return Json(new { success = result.Succeeded });
    }

    [HttpPost("delete")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _projectService.DeleteProjectAsync(id);
        return Json(new { success = result.Succeeded });
    }
}
