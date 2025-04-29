
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class EditProjectFormData
{
    public int Id { get; set; }
    [Display(Name = "Project Image", Prompt = "Select an image")]
    [DataType(DataType.Upload)]
    public string? Image { get; set; }

    [Display(Name = "Project Name", Prompt = "Project Name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string ProjectName { get; set; } = null!;

    [Display(Name = "Client Name", Prompt = "Client Name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string ClientName { get; set; } = null!;

    [Display(Name = "Status")]
    public string Status { get; set; } = null!;

    [Display(Name = "Description", Prompt = "Type something")]
    [DataType(DataType.Text)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Start Date", Prompt = "Start Date")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "Required")]
    [Display(Name = "End Date", Prompt = "End Date")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    [Display(Name = "Budget", Prompt = "0")]
    [DataType(DataType.Currency)]
    public decimal? Budget { get; set; }

    [Display(Name = "Members")]
    public List<string> SelectedMemberIds { get; set; } = new List<string>();

    public List<Member> AvailableMembers { get; set; } = new List<Member>();

    public List<Member> ProjectMembers { get; set; } = new List<Member>();
}
