
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;


public class StatusEntity
{
    [Key]
    public int Id { get; set; } 
    [Index(nameof(StatusName), IsUnique = true)]
    public string StatusName { get; set; } = null!;
    public virtual ICollection<ProjectEntity> Projects { get; set; } = [];
}