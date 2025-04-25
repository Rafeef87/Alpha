using Business.Services;
using Data.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin")]
public class AdminController(IMemberService memberService, IUserService userService) : Controller
{
    private readonly IMemberService _memberService = memberService;
    private readonly IUserService _userService = userService;

    [AllowAnonymous]
    //[Authorize(Roles = "admin")]
    [Route("members")]
    public async Task<IActionResult> Members()
    {
        var model = new MembersViewModel();
        var result = await _memberService.GetAllMembersAsync();

        if (result.Succeeded && result.Result != null)
        {
            model.Members = result.Result.Select(m => m.MapTo<MemberViewModel>()).ToList();
        }
        else
        { 
            ModelState.AddModelError("", result.Error ?? "Failed to load members");
        }

        return View(model);
    }
      

    [HttpPost]
    [Route("add-member")]
    public async Task<IActionResult> AddMember(AddMemberFormData form)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("members");

        var result = await _memberService.CreateMemberAsync(form);

        if (!result.Succeeded)
        {
            TempData["Error"] = result.Error;
            return RedirectToAction("Members");
        }

        TempData["Success"] = "Member added successfully!";
        return RedirectToAction("Members");
    }
    [HttpPost]
    [Route("edit-member")]
    public IActionResult EditMember(EditMemberFormData form)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("members");

        return View();
    }

    /* clients */
    [AllowAnonymous]
    [Route("clients")]
    public IActionResult Clients()
    {

        return View();
    }

    [HttpPost]
    [Route("add-client")]
    public IActionResult AddClient(AddClientFormData form)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Clients");

        return View();
    }
    [HttpPost]
    [Route("edit-client")]
    public IActionResult EditClient(EditClientFormData form)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Clients");

        return View();
    }
}
