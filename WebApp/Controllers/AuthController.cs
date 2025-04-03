
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class AuthController : Controller
{
    public IActionResult Register()
    {
        return View();
    }
    public IActionResult Login()
    {
        //return LocalRedirect("/projects");
        return View();
    }
    [HttpPost]
    public IActionResult Login(MemberLoginForm form)
    {

        return View();
    }
}
