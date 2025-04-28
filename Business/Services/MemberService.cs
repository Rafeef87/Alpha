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
        if (member == null)
            return false;

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
            // Create UserEntity
            var userEntity = new UserEntity
            {
                UserName = formData.Email,
                Email = formData.Email,
                FirstName = formData.FirstName,
                LastName = formData.LastName,
                PhoneNumber = formData.PhoneNumber
            };


            // create user throw UserManager
            var createResult = await _userManager.CreateAsync(userEntity, formData.Password);

            if (!createResult.Succeeded)
            {
                var errorMessages = string.Join(", ", createResult.Errors.Select(e => e.Description));
                return new MemberResult { Succeeded = false, StatusCode = 400, Error = errorMessages };
            }

            // add roll
            var roleResult = await _userManager.AddToRoleAsync(userEntity, formData.Role);

            if (!roleResult.Succeeded)
            {
                var errorMessages = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                return new MemberResult { Succeeded = false, StatusCode = 400, Error = "User created, but failed to add role: " + errorMessages };
            }

            return new MemberResult { Succeeded = true, StatusCode = 201 };
        }
        catch (Exception)
        {
           
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

