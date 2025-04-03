
using Domain.Models;
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
        ViewBag.ErrorMessage = null;
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(MemberLoginForm form, string returnUrl = "~/")
    {
        ViewBag.ErrorMessage = null;

        if (string.IsNullOrWhiteSpace(form.Email) || string.IsNullOrWhiteSpace(form.Password))
        {
            ViewBag.ErrorMessage = "Incorrect email or password.";
            return View(form);
        }
        if (!await _authService.UserExists(form.Email))
        {
            ViewBag.ErrorMessage = "The email provided is not found. Please sign up to get access to Alpha Admin Portal.";
            return View(form);
        }
        return View();
    }
}
