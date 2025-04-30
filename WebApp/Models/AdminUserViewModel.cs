using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class AdminUserViewModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string JobTitle { get; set; } = null!;
   
    public string ProfileImageUrl { get; set; } = null!;   

}
