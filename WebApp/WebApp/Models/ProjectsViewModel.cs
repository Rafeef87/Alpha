
using Business.Models;
using Domain.Models;

namespace WebApp.Models;

public class ProjectsViewModel
{
    //public Project Project { get; set; } = new();
    public IEnumerable<Project> Projects { get; set; } = [];
    public IEnumerable<User> Users { get; set; } = [];

    //public ProjectResult<Project> Project { get; internal set; } = null!;
    //public ClientResult Clients { get; internal set; } = null!;

    //public StatusResult<IEnumerable<Status>> Statuses { get; internal set; } = null!;
}