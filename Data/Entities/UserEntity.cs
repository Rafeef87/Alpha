using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Data.Entities;

public class UserEntity : IdentityUser
{
    [ProtectedPersonalData]
    public string? FirstName { get; set; }
    [ProtectedPersonalData]
    public string? LastName { get; set; }
    public string? JobTitle { get; set; }
    public virtual ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();
    public string? ProfileImageUrl { get; set; } 

    public ICollection<NotificationDismissedEntity> DismissedNotifications { get; set; } = [];
}
