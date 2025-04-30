using Business.Services;
using Data.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Models;

namespace WebApp.Controllers;

[Authorize]
[Route("admin")]
public class AdminController(UserManager<UserEntity> userManager, IMemberService memberService) : Controller
{

    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly IMemberService _memberService = memberService;



    // members

    [Authorize(Roles = "admin")]
    [Route("admin/members")]
    public async Task<IActionResult> Members()
    {
        var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");

        return View(adminUsers);
        //var members = await _memberService.GetAllMembersAsync();

        //var viewModel = new MembersViewModel
        //{
        //    Members = members.Select(m => new MemberViewModel
        //    {
        //        Id = int.TryParse(m.Id, out var id) ? id : 0,
        //        FirstName = m.FirstName!,
        //        LastName = m.LastName!,
        //        Email = m.Email!,
        //        PhoneNumber = m.PhoneNumber!,
        //        JobTitle = m.JobTitle!,
        //    }).ToList()
        //};

        //return View(viewModel);
    }
    //public IActionResult AddMember()
    //{
    //    var viewModel = new AddMemberFormData
    //    {
    //        Role = "Admin" // Default role
    //    };
    //    return View(viewModel);
    //}
    public IActionResult AddMember()
    {
        ViewBag.Roles = new List<SelectListItem>
       {
           new SelectListItem { Text = "Admin", Value = "Admin" },
           new SelectListItem { Text = "User", Value = "User" }
       };
        return View();
    }


    [HttpPost]
    [Route("edit-member")]
    public async Task<IActionResult> EditMember(string id)
    {
        var member = await _memberService.GetMemberByIdAsync(id);
        if (member == null)
            return NotFound();

        return View(member);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UserEntity member)
    {
        if (!ModelState.IsValid)
            return View(member);

        var updated = await _memberService.UpdateMemberAsync(member);

        if (!updated)
            return BadRequest();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Route("members")]
    public async Task<IActionResult> DeleteMember(string id)
    {
        await _memberService.DeleteMemberAsync(id);
        return RedirectToAction("Members", "Admin");

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
