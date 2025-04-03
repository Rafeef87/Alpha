
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class MemberLoginForm
{

    [Display(Name = "Email", Prompt = "Enter email address")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }


    [Display(Name = "Password", Prompt = "Enter Password")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

}
