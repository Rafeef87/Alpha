namespace WebApp.Models;

public class MemberViewModel
{
    public int Id { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string JobTitle { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string address { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
}
