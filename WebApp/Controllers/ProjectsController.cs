using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class ProjectsController : Controller
{
    [Authorize]
    [Route("projects")]
    public IActionResult Projects()
    {
        return View();
    }
}
