using Microsoft.AspNetCore.Http;

namespace Domain.Models;

public class User
{
    public string Id { get; set; } = null!;
    public string? Image { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public IEnumerable<object>? UserRoles { get; set; }
}
