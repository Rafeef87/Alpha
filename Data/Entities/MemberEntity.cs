using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Data.Entities;

public class MemberEntity 
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    [ProtectedPersonalData]
    public string? FirstName { get; set; }
    [ProtectedPersonalData]
    public string? LastName { get; set; }
    [ProtectedPersonalData]
    public string? JobTitle { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public virtual MemberAddressEntity? Address { get; set; }
}
