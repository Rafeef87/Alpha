using Business.Services;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

[Authorize]
public class ProjectsController(IProjectService projectService, IClientService clientService, IUserService userService, IStatusService statusService) : Controller
{
    private readonly IProjectService _projectService = projectService;
    private readonly IClientService _clientService = clientService;
    private readonly IUserService _userService = userService;
    private readonly IStatusService _statusService = statusService;

    [HttpGet]
    [Route("projects")]
    public async Task<IActionResult> Projects(string id)
    {
        var model = new ProjectsViewModel
        {
            Project = await _projectService.GetProjectAsync(id),
            Clients = await _clientService.GetClientsAsync(),
            Users = await _userService.GetUsersAsync(),
            Statuses = await _statusService.GetStatusesAsync()
        };
        return View(model);
    }
    //public async Task<IActionResult> Projects() 
    //{
    //    var model = new ProjectsViewModel
    //    {
    //        Projects = await _projectService.GetProjectsAsync() 
    //    };
    //    return View(model);
    //}

    [HttpPost("add")]
    public async Task<IActionResult> Add(AddProjectViewModel model)
    {
        if (!ModelState.IsValid)
            return Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors) });

        var form = model.MapTo<AddProjectFormData>();
        var result = await _projectService.CreateProjectAsync(form);

        return Json(new { success = result.Succeeded });
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update(EditProjectViewModel model)
    {
        if (!ModelState.IsValid)
            return Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors) });

        var form = model.MapTo<EditProjectFormData>();
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
