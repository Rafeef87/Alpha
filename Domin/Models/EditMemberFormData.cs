
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Models;

public class EditMemberFormData
{
    public string Id { get; set; } = null!;

    [Display(Name = "Member Image")]
    [DataType(DataType.Upload)]
    public IFormFile? Image { get; set; }

    [Display(Name = "First Name", Prompt = "Your First Name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Last Name", Prompt = "Your Last Name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string LastName { get; set; } = null!;
    public string? JobTitle { get; set; }

    [Display(Name = "Email", Prompt = "Your email address")]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^[^@\s]+[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email")]
    public string Email { get; set; } = null!;

    [Display(Name = "Phone", Prompt = "Your phone number")]
    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Address", Prompt = "Your address")]
    [DataType(DataType.Text)]
    public string? Address { get; set; }

    [Display(Name = "Date Of Birth", Prompt = "Date Of Birth")]
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }
}