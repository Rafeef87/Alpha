using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace WebApp.Models;

public class EditProjectViewModel
{
    [Required]
    public string Id { get; set; } = null!;

    public IFormFile? ImageFile { get; set; }

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Project Name")]
    public string ProjectName { get; set; } = null!;

    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Start Date")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Display(Name = "End Date")]
    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    [Display(Name = "Budget")]
    public decimal? Budget { get; set; }

    [Required]
    [Display(Name = "Client")]
    public string ClientId { get; set; } = null!;

    [Required]
    [Display(Name = "Assigned User")]
    public string UserId { get; set; } = null!;

    [Required]
    [Display(Name = "Status")]
    public int StatusId { get; set; }

    public string? ExistingImagePath { get; set; }
}
