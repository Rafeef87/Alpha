using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class AddProjectViewModel
{
    [Display(Name = "Project Image", Prompt = "Select an image")]
    public IFormFile? ImageFile { get; set; }

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Project Name", Prompt = "Project Name")]
    public string ProjectName { get; set; } = null!;

    [Display(Name = "Description", Prompt = "Type something")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Start Date", Prompt = "Start Date")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Display(Name = "End Date", Prompt = "End Date")]
    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    [Display(Name = "Budget", Prompt = "0")]
    public decimal? Budget { get; set; }

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Client")]
    public string ClientId { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Assigned User")]
    public string UserId { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Status")]
    public int StatusId { get; set; }
}
