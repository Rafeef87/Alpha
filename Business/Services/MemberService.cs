using Business.Models;
using Data.Entities;
using Data.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Business.Services;

public interface IMemberService
{
    Task<MemberResult> CreateMemberAsync(AddMemberFormData formData);
    Task<bool> DeleteMemberAsync(string id);
    Task<List<UserEntity>> GetAllMembersAsync();
    Task<UserEntity?> GetMemberByIdAsync(string id);
    Task<bool> UpdateMemberAsync(UserEntity member);
}

public class MemberService(UserManager<UserEntity> userManager, IUserRepository userRepository) : IMemberService
{
   
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly IUserRepository _userRepository = userRepository;

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

    public async Task<MemberResult> CreateMemberAsync(AddMemberFormData formData)
    {
        if (formData == null)
            return new MemberResult { Succeeded = false, StatusCode = 400, Error = "Missing data" };
        if (string.IsNullOrWhiteSpace(formData.FirstName) || string.IsNullOrWhiteSpace(formData.LastName))
            return new MemberResult { Succeeded = false, StatusCode = 400, Error = "First name and last name are required." };

        try
        {
            var userEntity = new UserEntity
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = formData.FirstName,
                LastName = formData.LastName,
                Email = formData.Email,
                PhoneNumber = formData.PhoneNumber,
                JobTitle = formData.JobTitle
            };

            var result = await _userRepository.AddAsync(userEntity);

            return result.Succeeded
                ? new MemberResult { Succeeded = true, StatusCode = 201 }
                : new MemberResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
        }
        catch (Exception ex)
        {
            // Log the exception
            return new MemberResult { Succeeded = false, StatusCode = 500, Error = "An unexpected error occurred." };
        }
    }

    public async Task<bool> DeleteMemberAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return false;

        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded;
    }
}

