using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NotificationSchedulingSystem.Infrastructure.Models;


namespace NotificationSchedulingSystem.Business.Services;

public class CreateNotificationsService : ICreateNotificationsService
{
    private readonly List<Notification> _notifications = new();
    private readonly IConfiguration _configuration;
    private readonly ILogger<CreateNotificationsService> _logger;

    public CreateNotificationsService(IConfiguration configuration, ILogger<CreateNotificationsService> logger)
    {
        _configuration = configuration ??
                         throw new ArgumentException(
                             $"{GetType().Name} Initialization failure due to: {nameof(configuration)}");
        _logger = logger;
    }

    private bool CreateNotifications(Company company)
    {
        var retVal = false;

        try
        {
            var periodicityOfNotifications = _configuration.GetSection($"NotificationPeriodicity:{company.Market}:{company.CompanyType}").Get<List<int>>();
            if (periodicityOfNotifications != null && periodicityOfNotifications.Count > 0)
            {
                foreach (var addDays in periodicityOfNotifications)
                {
                    var notification = new Notification()
                    {
                        CompanyId = company.Id,
                        Company = company,
                        SendingDate = DateTime.Now.AddDays(addDays)
                    };

                    _notifications.Add(notification);
                }

                retVal = true;
            }
        }
        catch (Exception)
        {
            _logger.LogWarning("CreateNotificationsService - CreateNotifications");
        }

        return retVal;
    }

    public Company AddNotifications(Company company)
    {
        if (CreateNotifications(company))
        {
            company.Notifications = _notifications;
        }

        return company;
    }
}