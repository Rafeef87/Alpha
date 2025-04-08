
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;


public class ClientEntity
{
    [Key]
    public string Id { get; set; } = null!;

    [Index(nameof(ClientName), IsUnique = true)]
    public string ClientName { get; set; } = null!;
    public virtual ICollection<ProjectEntity> Projects { get; set; } = [];
}
