
using Business.Models;
using Domain.Models;

namespace WebApp.Models;

public class ProjectsViewModel
{
    public ProjectResult<IEnumerable<Project>> Projects { get; set; } = null!;
    public IEnumerable<User> Users { get; set; } = null!;

    public ProjectResult<Project> Project { get; internal set; } = null!;
    public ClientResult Clients { get; internal set; } = null!;

    public StatusResult<IEnumerable<Status>> Statuses { get; internal set; } = null!;
}