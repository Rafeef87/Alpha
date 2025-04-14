
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Image {  get; set; }
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; }

    [Column(TypeName ="date")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime? EndDate { get; set; }

    public decimal? Budget {  get; set; }
    public DateTime Created {  get; set; } = DateTime.Now;

    // Foreign key for Client
    [Required]
    public string ClientId { get; set; } = default!;
    [ForeignKey(nameof(ClientId))]
    public virtual ClientEntity Client { get; set; } = default!;

    // Foreign key for User
    [Required]
    public string UserId { get; set; } = default!;
    [ForeignKey(nameof(UserId))]
    public virtual UserEntity User { get; set; } = default!;

    // Foreign key for Status
    [Required]
    public int StatusId { get; set; }
    [ForeignKey(nameof(StatusId))]
    public virtual StatusEntity Status { get; set; } = default!;
}
