using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Models;

public class UserRegistrationViewModel
{
    [DataType(DataType.Upload)]
    public IFormFile Image { get; set; } = null!;

    [Display(Name = "First Name", Prompt = "Your First Name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Last Name", Prompt = "Your Last Name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Email", Prompt = "Your email address")]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = null!;

    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Your Password")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, and one number.")]
    [Required(ErrorMessage = "Required")]
    public string Password { get; set; } = null!;

    [Display(Name = "Role", Prompt = "Select User Role")]
    [Required(ErrorMessage = "Required")]
    public string RoleId { get; set; } = null!;

    [ForeignKey(nameof(IdentityRole))]
    public string Role { get; set; } = null!;

    [Display(Name = "Phone Number", Prompt = "Your Phone Number")]
    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }
    [Display(Name = "Address", Prompt = "Your Address")]
    [DataType(DataType.Text)]
    public string? Address { get; set; }

    [Display(Name = "Date Of Birth", Prompt = "Your Date Of Birth")]
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    [Display(Name = "Job Title", Prompt = "Your Job Title")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string JobTitle { get; set; } = null!;
}