
namespace NotificationSchedulingSystem.Business.Models;

public class CompanyResponse
{
    public Guid CompanyId { get; set; }
    public List<string>? Notifications { get; set; }
}