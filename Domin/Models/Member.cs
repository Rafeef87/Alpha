
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Models;

public class Member
{

    public string? Id { get; set; }
    public IFormFile? Image { get; set; }
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
   
    public string? JobTitle { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }

    public virtual MemberAddress? Address { get; set; } = new();
}
