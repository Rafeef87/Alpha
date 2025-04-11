using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers;

[Authorize]
public class UsersController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager) : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly UserManager<IdentityUser> _userManager = userManager;
    
    public async Task<IActionResult> Index()
    {
        ViewBag.Roles = await _roleManager.Roles.Select(x => new SelectListItem {  Value = x.Name, Text = x.Name}).ToListAsync();
        return View();
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost]
    public async Task<IActionResult> Index(UserRegistrationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Roles = await _roleManager.Roles.Select(x => new SelectListItem { Value = x.Name, Text = x.Name }).ToListAsync();
            return View(model);
        }

        var user = new IdentityUser
        {
            UserName = model.Email,
            Email = model.Email,
        };
        var identityResult = await _userManager.CreateAsync(user);
        if (!identityResult.Succeeded)
        {
            ViewBag.ErrorMessage = "Unable to create User.";
            ViewBag.Roles = await _roleManager.Roles.Select(x => new SelectListItem { Value = x.Name, Text = x.Name }).ToListAsync();
            return View(model);
        }
        var roleResult = await _userManager.AddToRoleAsync(user, model.Role);
        if (!roleResult.Succeeded)
        {
            ViewBag.ErrorMessage = "Unable to add Role to User .";
            ViewBag.Roles = await _roleManager.Roles.Select(x => new SelectListItem { Value = x.Name, Text = x.Name }).ToListAsync();
            return View(model);
        }
            return RedirectToAction("Index");
    }

}
