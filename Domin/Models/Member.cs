
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Models;

public class Member
{
    public string? Id { get; set; }
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;  //Navigation properties
    public string Title { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
}
