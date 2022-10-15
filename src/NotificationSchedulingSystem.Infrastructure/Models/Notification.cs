namespace NotificationSchedulingSystem.Infrastructure.Models;

public class Notification
{
    public int Id { get; set; }
    public DateTime SendingDate { get; set; }
    public Guid CompanyId { get; set; }
    public virtual Company? Company { get; set; }
}