
using Business.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class AuthController(IAuthService authService) : Controller
{
    private readonly IAuthService _authService = authService;

    #region Sign up
    public IActionResult SignUp()
    {       
        ViewBag.ErrorMessage = "";
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> SignUp(MemberSignUpForm form)
    {
        ViewBag.ErrorMessage = "";

        if (ModelState.IsValid)
        {
            var result = await _authService.SignUpAsync(form);
            if (result)
                return LocalRedirect("~/");

            // Om användaren inte kunde skapas – visa fel
            ViewBag.ErrorMessage = "Could not create account. Please check the form or try another email.";
        }
        else
        {
            ViewBag.ErrorMessage = "Please correct the highlighted errors.";
        }

        return View(form); // alltid en return!
    }
    #endregion

    #region login
    public IActionResult Login()
    {
      
        ViewBag.ErrorMessage = "";
        //return LocalRedirect("~/");
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
                if (string.IsNullOrWhiteSpace(returnUrl) || !Url.IsLocalUrl(returnUrl))
                    return RedirectToAction("Projects", "Projects");
                return Redirect(returnUrl);
            }
        }

            ViewBag.ErrorMessage = "Incorrect email or password.";
            return View(form);
    }
#endregion
}
