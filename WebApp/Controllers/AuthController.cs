
using Business.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class AuthController(IAuthService authService) : Controller
{
    private readonly IAuthService _authService = authService;

    #region
    public IActionResult SignUp()
    {       
        ViewBag.ErrorMessage = "";
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> SignUp(MemberSignUpForm form)
    {
        ViewBag.ErrorMessage = "";

       
        return View(form);
    }
    #endregion

    #region login
    public IActionResult Login()
    {
        //return LocalRedirect("/projects");
        ViewBag.ErrorMessage = "";
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(MemberLoginForm form, string returnUrl = "~/")
    {
        ViewBag.ErrorMessage = "";

        if (ModelState.IsValid)
        {
            var result = await _authService.LoginAsync(form);
            if (result)
            {
                return Redirect(returnUrl);
            }
        }

            ViewBag.ErrorMessage = "Incorrect email or password.";
            return View(form);
    }
#endregion
}
