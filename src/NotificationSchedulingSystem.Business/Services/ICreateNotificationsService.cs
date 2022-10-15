using NotificationSchedulingSystem.Infrastructure.Models;

namespace NotificationSchedulingSystem.Business.Services;

public interface ICreateNotificationsService
{
    Company AddNotifications(Company company);
}