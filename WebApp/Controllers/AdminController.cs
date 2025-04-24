using Business.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Authorize]
[Route("admin")]
public class AdminController(IMemberService memberService, IUserService userService) : Controller
{
    private readonly IMemberService _memberService = memberService;
    private readonly IUserService _userService = userService;

    [AllowAnonymous]
    //[Authorize(Roles = "admin")]
    [Route("members")]
    //public IActionResult Members()
    //{

    //    return View();
    //}
    public async Task<IActionResult> Members()
    {
        var members = await _memberService.GetAllMembers();
        return View(members);
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
