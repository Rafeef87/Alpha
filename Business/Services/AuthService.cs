
using Data.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public interface IAuthService
{
    Task<bool> LoginAsync(MemberLoginForm loginForm);
    Task<bool> SignUpAsync(MemberSignUpForm loginForm);
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
}
