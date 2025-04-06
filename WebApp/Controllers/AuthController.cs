
using System.Security.Claims;
using Business.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class AuthController(IAuthService authService) : Controller
{
    private readonly IAuthService _authService = authService;

   #region Local Identity
    #region Sign up

    [HttpGet]
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
        }
        ViewBag.ErrorMessage = "Unable to create account.";
        return View(form);
    }
    #endregion

    #region login

    [HttpGet]
    public IActionResult Login(string returnUrl = "~/")
    {
        ViewBag.ReturnUrl = returnUrl;
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
                return Redirect(returnUrl);
        }
            ViewBag.ErrorMessage = "Incorrect email or password.";
            return View(form);
    }
    #endregion
    #region Sign Out

    #endregion
    #endregion

    #region External Authentication
    [HttpPost]
    public async Task<IActionResult> ExternalSignIn(string provider, string returnUrl = "/")
    {
        if (string.IsNullOrWhiteSpace(provider))
        {
            ModelState.AddModelError("", "Invalid provider");
            return View("Login");
        }

        var redirectUrl = Url.Action("ExternalSignInCallBack", "Auth", new { returnUrl })!;
        var properties = await _authService.GetExternalLoginPropertiesAsync(provider, redirectUrl);

        return Challenge(properties, provider);
    }

    public async Task<IActionResult> ExternalSignInCallback(string? returnUrl = null)
    {
        var info = await _authService.GetExternalLoginInfoAsync();
        if (info == null)
            return RedirectToAction("Login");

        var result = await _authService.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey);

        if (result.Succeeded)
        {
            return RedirectToLocal(returnUrl);
        }

        var createResult = await _authService.CreateUserFromExternalLoginAsync(info);
        if (createResult.Succeeded)
        {
            await _authService.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey);
            return RedirectToLocal(returnUrl);
        }

        // Misslyckades
        foreach (var error in createResult.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View("Login");
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl!);

        return RedirectToAction("Auth", "Login");
    }
    #endregion

}