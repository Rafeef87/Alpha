using System.Security.Claims;
using Business.Services;
using Data.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers;

[Authorize(Roles = "Admin")]

public class UsersController(RoleManager<IdentityRole> roleManager, UserManager<UserEntity> userManager, IUserService userService) : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly IUserService _userService = userService;

    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.ToListAsync();

        var viewModel = new List<AdminUserViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user); // Retrieves roles for each user
            viewModel.Add(new AdminUserViewModel
            {
                Name = $"{user.FirstName} {user.LastName}",
                Email = user.Email!,
                Role = roles.FirstOrDefault() ?? "No Role", //Takes the first role if several
                Phone = user.PhoneNumber!
            });
        }

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Roles = await _roleManager.Roles
            .Select(x => new SelectListItem { Value = x.Name, Text = x.Name })
            .ToListAsync();

        return View(new UserRegistrationViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserRegistrationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Roles = await _roleManager.Roles.Select(x => new SelectListItem { Value = x.Name, Text = x.Name }).ToListAsync();
            return View(model);
        }
        // Check if the role exists
        if (!await _roleManager.RoleExistsAsync(model.Role))
        {
            ViewBag.ErrorMessage = "The selected role does not exist.";
            ViewBag.Roles = await _roleManager.Roles.Select(x => new SelectListItem { Value = x.Name, Text = x.Name }).ToListAsync();
            return View(model);
        }
        var user = new UserEntity
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
    [HttpPost]
    public async Task<IActionResult> UploadProfileImage(IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            var filePath = Path.Combine("wwwroot/images", file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                user.ProfileImageUrl = $"/images/{file.FileName}";
                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction("Profile");
        }

        return View();
    }



    public async Task<IActionResult> Add(UserRegistrationViewModel form, string roleName = "Admin")
    {
        // Map UserRegistrationViewModel to SignUpFormData
        var signUpFormData = new SignUpFormData
        {
            FirstName = form.FirstName,
            LastName = form.LastName,
            Email = form.Email,
            Password = form.Password,
        };

        var user = await _userService.CreateUserAsync(signUpFormData, roleName);

        return RedirectToAction("Index");
    }



    //[HttpPost]
    //[Route("edit-member")]
    //public async Task<IActionResult> Edit(string id)
    //{
    //    var member = await _memberService.GetMemberByIdAsync(id);
    //    if (member == null)
    //        return NotFound();

    //    return View(member);
    //}

    //[HttpPost]
    //public async Task<IActionResult> Edit(UserEntity member)
    //{
    //    if (!ModelState.IsValid)
    //        return View(member);

    //    var updated = await _memberService.UpdateMemberAsync(member);

    //    if (!updated)
    //        return BadRequest();

    //    return RedirectToAction(nameof(Index));
    //}

    //[HttpPost]
    //[Route("members")]
    //public async Task<IActionResult> DeleteMember(string id)
    //{
    //    await _memberService.DeleteMemberAsync(id);
    //    return RedirectToAction("Members", "Admin");

    //}
}
