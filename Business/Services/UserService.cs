
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
    Task<UserResult> CreateUserAsync(AddUserFormData formData);
    Task<UserResult> GetUserAsync();
    Task<UserResult> UpdateUserAsync(EditUserFormData formData);
    Task<UserResult> DeleteUserAsync(string id);

}

public class UserService(IUserRepository userRepository, UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    #region CRUD
    public async Task<UserResult> CreateUserAsync(AddUserFormData formData)
    {
        if (formData == null)
            return new UserResult { Succeeded = false, StatusCode = 400, Error = "Not all required field are supplied." };
        
        var userEntity = formData.MapTo<UserEntity>();
        var result = await _userRepository.AddAsync(userEntity);
       
        return result.Succeeded
            ? new UserResult { Succeeded = true, StatusCode = 200 }
            : new UserResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }

    public async Task<UserResult> GetUserAsync()
    {
        var result = await _userRepository.GetAllAsync();
        return result.MapTo<UserResult>();
    }

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
        var userEntity = new UserEntity { Id = id};
        var result = await _userRepository.DeleteAsync(userEntity);

        return result.Succeeded
            ? new UserResult { Succeeded = true, StatusCode = 200 }
            : new UserResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }
    #endregion
}
