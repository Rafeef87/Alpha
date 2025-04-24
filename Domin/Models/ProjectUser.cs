
namespace Domain.Models;

public class ProjectUser
{
    public int ProjectId { get; set; }
    public string Id { get; set; } = null!;

    public virtual Project Project { get; set; }
    public virtual User User { get; set; }
}
