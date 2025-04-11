using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class MemberSignUpForm
{
    [Display(Name = "Full Name", Prompt = "Your full name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string FullName { get; set; } = null!;

    [Display(Name = "Email", Prompt = "Your email address")]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Password", Prompt = "Enter your password")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "Invalid password")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Compare(nameof(Password), ErrorMessage = "Passwords don't match!")]
    [Display(Name = "Confirm Password", Prompt = "Confirm your password")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = null!;

}
