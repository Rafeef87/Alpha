
using System.Security.Claims;
using Business.Models;
using Data.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public interface IAuthService
{
    Task<IdentityResult> CreateUserFromExternalLoginAsync(ExternalLoginInfo info);
    Task<SignInResult> ExternalLoginSignInAsync(string provider, string providerKey);
    Task<ExternalLoginInfo?> GetExternalLoginInfoAsync();
    Task<AuthenticationProperties> GetExternalLoginPropertiesAsync(string provider, string redirectUrl);
    Task<AuthResult> SigInAsync(SignInFormData formData);
    Task<AuthResult> SignOutAsync();
    Task<AuthResult> SignUpAsync(SignUpFormData signupForm);
}

public class AuthService(IUserService userService, SignInManager<UserEntity> signInManager) : IAuthService
{
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly IUserService _userService = userService;

    // login 
    public async Task<AuthResult> SigInAsync(SignInFormData formData)
    {

        if (formData == null)
            return new AuthResult { Succeeded = false, StatusCode = 400, Error = "Not all required field are supplied." };

        var result = await _signInManager.PasswordSignInAsync(formData.Email, formData.Password, formData.IsPersisten, false);
        return result.Succeeded
              ? new AuthResult { Succeeded = true, StatusCode = 200 }
              : new AuthResult { Succeeded = false, StatusCode = 401, Error = "Invalid email or password." };
    }
    //signup 
    public async Task<AuthResult> SignUpAsync(SignUpFormData signupForm)
    {
        if (signupForm == null)
            return new AuthResult { Succeeded = false, StatusCode = 400, Error = "Not all required field are supplied." };

        var result = await _userService.CreateUserAsync(signupForm);
        return result.Succeeded
              ? new AuthResult { Succeeded = true, StatusCode = 201 }
              : new AuthResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }

    // logout
    public async Task<AuthResult> SignOutAsync()
    {
        await _signInManager.SignOutAsync();
        return new AuthResult { Succeeded = true, StatusCode = 200 };
    }

    #region External Authentication
    public Task<AuthenticationProperties> GetExternalLoginPropertiesAsync(string provider, string redirectUrl)
    {
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Task.FromResult(properties);
    }

    public async Task<ExternalLoginInfo?> GetExternalLoginInfoAsync()
    {
        return await _signInManager.GetExternalLoginInfoAsync();
    }

    public async Task<SignInResult> ExternalLoginSignInAsync(string provider, string providerKey)
    {
        return await _signInManager.ExternalLoginSignInAsync(provider, providerKey, isPersistent: false, bypassTwoFactor: true);
    }
    /* Regenerate med hjälp av ChatGPT */
    public async Task<IdentityResult> CreateUserFromExternalLoginAsync(ExternalLoginInfo info)
    {
        if (info == null)
            return IdentityResult.Failed(new IdentityError { Description = "External login info is null." });

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        var name = info.Principal.FindFirstValue(ClaimTypes.Name) ?? "Unknown";
        var username = email ?? $"{info.LoginProvider}_{info.ProviderKey}";

        //If the user already exists - just add external login
        var existingUser = await _signInManager.UserManager.FindByEmailAsync(email!);
        if (existingUser != null)
        {
            var result = await _signInManager.UserManager.AddLoginAsync(existingUser, info);
            return result;
        }

        // Create users via UserService
        var formData = new SignUpFormData
        {
            Email = email!,
            FirstName = username,
            LastName = name,
        };

        var userResult = await _userService.CreateUserAsync(formData);
        if (!userResult.Succeeded)
        {
            return IdentityResult.Failed(new IdentityError { Description = userResult.Error ?? "Failed to create user." });
        }

        // Get the new user
        var newUser = await _signInManager.UserManager.FindByEmailAsync(email!);
        if (newUser == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User creation succeeded but user could not be found." });
        }

        // Add external login to the user
        return await _signInManager.UserManager.AddLoginAsync(newUser, info);
    }

    #endregion

}
