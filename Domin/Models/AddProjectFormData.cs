
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Models;

public class AddProjectFormData
{
    [Display(Name = "Project Image", Prompt = "Select an image")]
    [DataType(DataType.Upload)]
    public string? Image { get; set; }

    [Display(Name = "Project Name", Prompt = "Project Name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string ProjectName { get; set; } = null!;

    [Display(Name = "Description", Prompt = "Type something")]
    [DataType(DataType.Text)]
    public string? Description { get; set; }
    [Display(Name = "Start Date", Prompt = "Start Date")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Display(Name = "End Date", Prompt = "End Date")]
    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    [Display(Name = "Budget", Prompt = "0")]
    [DataType(DataType.Text)]
    public decimal? Budget { get; set; }
    
    [Display(Name = "Client Name", Prompt = "Client Name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string ClientId { get; set; } = null!;

    [Display(Name = "Members", Prompt = "Members")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string UserId { get; set; } = null!;

    [Display(Name = "Status", Prompt = "Status")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public int StatusId { get; set; }
}
