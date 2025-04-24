namespace Domain.Models;

public class Project
{
    public int ProjectId { get; set; }
    public string? Image { get; set; }
    public string ProjectName { get; set; } = null!;
    public string ClientName { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime Deadline { get; set; }
    //public decimal? Budget { get; set; }
    public virtual ICollection<ProjectUser> ProjectUsers { get; set; } = [];
    public int DaysLeft => (int)Math.Max(0, (Deadline - DateTime.Today).TotalDays);
    //public bool IsUrgent => DaysLeft <= 7 && DaysLeft > 0;

}