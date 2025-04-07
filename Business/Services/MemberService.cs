

using Data.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public interface IMemberService
{
    Task<IEnumerable<Member>> GetAllMembers();
}
public class MemberService(UserManager<MemberEntity> userManager) : IMemberService
{
    private readonly UserManager<MemberEntity> _userManager = userManager;

    public async Task<IEnumerable<Member>> GetAllMembers()
    {
        var list = await _userManager.Users.ToListAsync();
        var members = list.Select(x => new Member
        {
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Email = x.Email,
            Phone = x.PhoneNumber,
            JobTitle = x.JobTitle,
        });
        return members;
    }
}

