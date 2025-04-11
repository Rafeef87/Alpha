
using System.Security.Claims;
using Business.Services;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using WebApp.Models;

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
    public async Task<IActionResult> SignUp(SignUpViewModel model)
    {
        ViewBag.ErrorMessage = "";

        if (!ModelState.IsValid)
            return View(model);

        var formData = model.MapTo<SignUpFormData>();
        var result = await _authService.SignUpAsync(formData);
       
           
        if (!result.Succeeded)
        {
            ViewBag.ErrorMessage = result.Error;
            return View(model);
        }
        return RedirectToAction("SignIn", "Auth"); 
        
    }
    #endregion

    #region login

    [HttpGet]
    public IActionResult SignIn(string returnUrl = "~/")
    {
        ViewBag.ErrorMessage = "";
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> SignIn(SignInViewModel model, string returnUrl = "~/")
    {
        ViewBag.ErrorMessage = "";
        ViewBag.ReturnUrl = returnUrl;

        if (!ModelState.IsValid)
            return View(model);

        var signInFormData = model.MapTo<SignInFormData>();
        var result = await _authService.SignInAsync(signInFormData);
        Console.WriteLine($"Email: {model.Email}, Password: {model.Password}, Remember me: {model.IsPersisten}");

        if (!result.Succeeded)
        {
            ViewBag.ErrorMessage = result.Error;
            return View(model);
        }
        return LocalRedirect(returnUrl);
    }
    #endregion
    #region Sign Out
    public async Task<IActionResult> Logout()
    {
        await _authService.SignOutAsync();
        return LocalRedirect("~/");
    }
    #endregion
    #endregion

    #region External Authentication
    [HttpPost]
    public async Task<IActionResult> ExternalSignIn(string provider, string returnUrl = "/")
    {
        if (string.IsNullOrWhiteSpace(provider))
        {
            ModelState.AddModelError("", "Invalid provider");
            return View("SignIn");
        }

        var redirectUrl = Url.Action("ExternalSignInCallBack", "Auth", new { returnUrl })!;
        var properties = await _authService.GetExternalLoginPropertiesAsync(provider, redirectUrl);

        return Challenge(properties, provider);
    }

    public async Task<IActionResult> ExternalSignInCallback(string? returnUrl = null)
    {
        var info = await _authService.GetExternalLoginInfoAsync();
        if (info == null)
            return RedirectToAction("SignIn");

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

        return View("SignIn");
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl!);

        return RedirectToAction("Projects", "Projects");
    }

    [HttpPost]
    public async Task<IActionResult> ExternalSignUp(SignUpFormData model, string returnUrl = "~/")
    {
        ViewBag.ErrorMessage = "";
        ViewBag.ReturnUrl = returnUrl;

        if (!ModelState.IsValid)
            return View(model);

        var externalLoginInfo = await _authService.GetExternalLoginInfoAsync();
        if (externalLoginInfo == null)
        {
            ViewBag.ErrorMessage = "External login information could not be retrieved.";
            return View(model);
        }

        var signUpFormData = model.MapTo<SignUpFormData>();
        var result = await _authService.CreateUserFromExternalLoginAsync(externalLoginInfo);

        if (result.Succeeded)
        {
            await _authService.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey);
            return LocalRedirect(returnUrl);
        }

        ViewBag.ErrorMessage = "Failed to sign up using external provider.";
        return View(model);
    }

    #endregion

}