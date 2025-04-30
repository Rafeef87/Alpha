using Microsoft.AspNetCore.Http;

namespace Domain.Models;

public class User
{
    public string Id { get; set; } = null!;
    public IFormFile? Image { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public IEnumerable<object>? UserRoles { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? JobTitle { get; set; }
}
