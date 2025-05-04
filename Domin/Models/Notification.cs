namespace Domain.Models;

public class Notification
{
    public int Id { get; set; }
    public string? Type { get; set; }
    public string? Message { get; set; }
    public string? Image { get; set; }

    public DateTime Created { get; set; }
}
