
namespace Domain.Models;

public class ProjectListViewModel
{
    public List<Project> AllProjects { get; set; } = new List<Project>();
    public List<Project> StartedProjects { get; set; } = new List<Project>();
    public List<Project> CompletedProjects { get; set; } = new List<Project>();
    public string ActiveFilter { get; set; } = "all";
}
