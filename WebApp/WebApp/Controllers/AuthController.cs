
using System.Security.Claims;
using Business.Services;
using Data.Entities;
using Data.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using WebApp.Models;

namespace WebApp.Controllers;

public class AuthController(IAuthService authService, SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager) : Controller
{
    private readonly IAuthService _authService = authService;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly UserManager<UserEntity> _userManager = userManager;

    // Denied non-admin user 
    [AllowAnonymous]
    
    public IActionResult Denied()
    {

        return View();
    }

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

   
        var result = await _authService.SignInAsync(model.Email, model.Password, model.IsPersisten);
        Console.WriteLine($"Email: {model.Email}, Password: {model.Password}, Remember me: {model.IsPersisten}");

        if (!result.Succeeded)
        {
            ViewBag.ErrorMessage = result.Error;
            return View(model);
        }
        // Hämta den inloggade användaren
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ViewBag.ErrorMessage = "User not found.";
            return View(model);
        }

        return LocalRedirect(returnUrl);
    }
    #endregion

    // login with email and password - admin
    [HttpGet]
    public IActionResult LogIn()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> LogIn(string email, string password, string returnUrl = "~/")
    {
        ViewBag.ErrorMessage = "";
        ViewBag.ReturnUrl = returnUrl;

        var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && await _userManager.IsInRoleAsync(user, "Admin")) 
            {
                return RedirectToAction("Index", "Users"); // Admin page
            }

            return LocalRedirect(returnUrl ?? "/");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View();
    }
    //public async Task<IActionResult> Login(SignInViewModel model)
    //{
    //    if (!ModelState.IsValid) return View(model);

    //    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.IsPersisten, false);
    //    if (result.Succeeded)
    //    {
    //        var user = await _userManager.FindByEmailAsync(model.Email);
    //        if (user != null && await _userManager.IsInRoleAsync(user, "Administrator"))
    //        {
    //            return RedirectToAction("Members", "Admin");
    //        }
    //        ModelState.AddModelError("", "Access denied. Admins only.");
    //    }
    //    else
    //    {
    //        ModelState.AddModelError("", "Invalid login attempt.");
    //    }
    //    return View(model);
    //}


    #region Sign Out
    public async Task<IActionResult> Logout()
    {
        await _authService.SignOutAsync();
        return RedirectToAction("SignIn", "Auth");
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