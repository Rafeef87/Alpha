using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Authorize]

public class ProjectsController : Controller
{
    [Route("projects")]
    public IActionResult Projects()
    {
        return View();
    }
}
