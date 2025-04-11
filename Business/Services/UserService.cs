
using System.Diagnostics;
using Business.Models;
using Data.Entities;
using Data.Repositories;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public interface IUserService
{
    Task<UserResult> AddUserToRole(string userId, string roleName);
    Task<UserResult> CreateUserAsync(SignUpFormData formData, string roleName = "User");
    Task<UserResult> DeleteUserAsync(string id);
    Task<UserResult> GetUserAsync();
    Task<UserResult> UpdateUserAsync(EditUserFormData formData);
}

public class UserService(IUserRepository userRepository, UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;


    public async Task<UserResult> AddUserToRole(string userId, string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
            return new UserResult { Succeeded = false, StatusCode = 404, Error = "Role dosn't exists." };

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return new UserResult { Succeeded = false, StatusCode = 404, Error = "User dosn't exists." };

        var result = await _userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded
            ? new UserResult { Succeeded = true, StatusCode = 200 }
            : new UserResult { Succeeded = false, StatusCode = 500, Error = "Unable to add user to role." };
    }

    #region CRUD
    public async Task<UserResult> CreateUserAsync(SignUpFormData formData, string roleName = "User")
    {
        if (formData == null)
            return new UserResult { Succeeded = false, StatusCode = 400, Error = "Form data can't be null." };

        var existsResult = await _userRepository.ExistsAsync(x => x.Email == formData.Email);
        if (existsResult.Succeeded)
            return new UserResult { Succeeded = false, StatusCode = 409, Error = "User with same email already exists." };

        try
        {
            var userEntity = formData.MapTo<UserEntity>();

            // 🟡 MAKE SURE THAT UserName IS SET
            userEntity.UserName = formData.Email;

            var result = await _userManager.CreateAsync(userEntity, formData.Password);

            if (result.Succeeded)
            {
                var addToRoleResult = await AddUserToRole(userEntity.Id, roleName);
                return addToRoleResult.Succeeded
                    ? new UserResult { Succeeded = true, StatusCode = 201 }
                    : new UserResult { Succeeded = false, StatusCode = 201, Error = "User created but not added to role." };
            }

            // 🟥 Return exactly why it failed
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new UserResult
            {
                Succeeded = false,
                StatusCode = 400,
                Error = $"Failed to create user: {errors}"
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new UserResult { Succeeded = false, StatusCode = 400, Error = ex.Message };
        }
    }

    public async Task<UserResult> GetUserAsync()
    {
        var result = await _userRepository.GetAllAsync();
        return result.MapTo<UserResult>();
    }

    public async Task<UserResult> UpdateUserAsync(EditUserFormData formData)
    {
        if (formData == null)
            return new UserResult { Succeeded = false, StatusCode = 400, Error = "Not all required field are supplied." };

        var userEntity = formData.MapTo<UserEntity>();
        var result = await _userRepository.UpdateAsync(userEntity);

        return result.Succeeded
            ? new UserResult { Succeeded = true, StatusCode = 200 }
            : new UserResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }
    public async Task<UserResult> DeleteUserAsync(string id)
    {
        var userEntity = new UserEntity { Id = id };
        var result = await _userRepository.DeleteAsync(userEntity);

        return result.Succeeded
            ? new UserResult { Succeeded = true, StatusCode = 200 }
            : new UserResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }
    #endregion
}
