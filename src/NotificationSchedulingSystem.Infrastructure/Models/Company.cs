using NotificationSchedulingSystem.Infrastructure.Enums;

namespace NotificationSchedulingSystem.Infrastructure.Models;

public class Company
{
    public Company()
    {
        Notifications = new List<Notification>();
    }

    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int Number { get; set; }
    public CompanyType CompanyType { get; set; }
    public Market Market { get; set; }
    public virtual List<Notification> Notifications { get; set; }

}