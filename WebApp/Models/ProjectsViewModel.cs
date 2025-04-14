
using Business.Models;
using Domain.Models;

namespace WebApp.Models;

public class ProjectsViewModel
{
    public ProjectResult<IEnumerable<Project>> Projects { get; set; } = null!;
    public ProjectResult<Project> Project { get; internal set; }
    public ClientResult Clients { get; internal set; }
    public UserResult Users { get; internal set; }
    public StatusResult<IEnumerable<Status>> Statuses { get; internal set; }
}