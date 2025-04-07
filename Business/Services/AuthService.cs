
using System.Security.Claims;
using Data.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public interface IAuthService
{
    Task<bool> LoginAsync(MemberLoginForm loginForm);
    Task<bool> SignUpAsync(MemberSignUpForm loginForm);
    Task LogoutAsync();
    Task<AuthenticationProperties> GetExternalLoginPropertiesAsync(string provider, string redirectUrl);
    Task<ExternalLoginInfo?> GetExternalLoginInfoAsync();
    Task<SignInResult> ExternalLoginSignInAsync(string provider, string providerKey);
    Task<IdentityResult> CreateUserFromExternalLoginAsync(ExternalLoginInfo info);

}

public class AuthService(SignInManager<MemberEntity> signInManager, UserManager<MemberEntity> userManager) : IAuthService
{
    private readonly SignInManager<MemberEntity> _signInManager = signInManager;
    private readonly UserManager<MemberEntity> _userManager = userManager;

    // login 
    public async Task<bool> LoginAsync(MemberLoginForm loginForm)
    {
        var user = await _userManager.FindByEmailAsync(loginForm.Email);

        if (user is null)
            return false;

        var result = await _signInManager.PasswordSignInAsync(
            user.UserName,
            loginForm.Password,
            loginForm.RememberMe, // ✅ Here we use the checkbox value
            lockoutOnFailure: false);
        return result.Succeeded;
    }
    //signup 

    public async Task<bool> SignUpAsync(MemberSignUpForm signupForm)
    {
        var memberEntity = new MemberEntity
        {
            UserName = signupForm.Email,
            FirstName = signupForm.FullName,
            LastName = signupForm.FullName,
            Email = signupForm.Email,
        };

        var result = await _userManager.CreateAsync(memberEntity, signupForm.Password);
        // Temporarily: log or forward the errors
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"{error.Code}: {error.Description}");
            }
        }

        return result.Succeeded;
    }

    // logout
    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    //External Authentication
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

    public async Task<IdentityResult> CreateUserFromExternalLoginAsync(ExternalLoginInfo info)
    {
        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        var name = info.Principal.FindFirstValue(ClaimTypes.Name);

        var user = new MemberEntity
        {
            UserName = email,
            Email = email,
            FirstName = name ?? email,
            LastName = name ?? email,
        };

        var result = await _userManager.CreateAsync(user);
        if (!result.Succeeded) return result;

        return await _userManager.AddLoginAsync(user, info);
    }


}
