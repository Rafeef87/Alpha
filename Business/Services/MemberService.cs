using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Business.Services;

public interface IMemberService
{
    Task<bool> CreateMemberAsync(UserEntity member, string password);
    Task<bool> DeleteMemberAsync(string id);
    Task<List<UserEntity>> GetAllMembersAsync();
    Task<UserEntity?> GetMemberByIdAsync(string id);
    Task<bool> UpdateMemberAsync(UserEntity member);
}

public class MemberService(UserManager<UserEntity> userManager) : IMemberService
{
   
    private readonly UserManager<UserEntity> _userManager = userManager;

    public async Task<List<UserEntity>> GetAllMembersAsync()
    {
        return await _userManager.Users.ToListAsync();
    }

    public async Task<UserEntity?> GetMemberByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task<bool> UpdateMemberAsync(UserEntity member)
    {
        var result = await _userManager.UpdateAsync(member);
        return result.Succeeded;
    }

    public async Task<bool> CreateMemberAsync(UserEntity member, string password)
    {
        var result = await _userManager.CreateAsync(member, password);
        return result.Succeeded;
    }

    public async Task<bool> DeleteMemberAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return false;

        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded;
    }
}

