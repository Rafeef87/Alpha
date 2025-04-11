using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers;
[Authorize]
public class UsersController(RoleManager<IdentityRole> roleManager) : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    public async Task<IActionResult> Index()
    {
        ViewBag.Roles = await _roleManager.Roles.Select(x => new SelectListItem {  Value = x.Name, Text = x.Name}).ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(UserRegistrationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Roles = await _roleManager.Roles.Select(x => new SelectListItem { Value = x.Name, Text = x.Name }).ToListAsync();
            return View(model);
        }
        return RedirectToAction("Index");
    }
}
