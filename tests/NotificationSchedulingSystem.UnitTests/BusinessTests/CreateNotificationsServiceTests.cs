using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NotificationSchedulingSystem.Business.Services;
using System.Text;
using NotificationSchedulingSystem.Infrastructure.Enums;
using NotificationSchedulingSystem.Infrastructure.Models;

namespace NotificationSchedulingSystem.UnitTests.BusinessTests;

public class CreateNotificationsServiceTests
{
    private CreateNotificationsService? _sut;
    private readonly Mock<IConfiguration> _configurationMock = new();
    private readonly Mock<ILogger<CreateNotificationsService>> _loggerMock = new();

    [Fact]
    public void Test_Constructor_When_DependenciesInitFailure_Result_Exception()
    {
        //Arrange
        Action act = null;

        //Act
        act = new Action(() =>
        {
            new CreateNotificationsService(null, null);
        });

        var exception = Record.Exception(act);

        //Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public void AddNotification_ReturnCompanyWithNotifications_WhenCreateNotificationsReturnTrue()
    {
        //arrange
        var appSettings = @"{""NotificationPeriodicity"": {
                ""Denmark"": {
                  ""Small"": [ 1, 5, 10, 15, 20 ]
                    }}}";

        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(appSettings)));
        var configuration = configurationBuilder.Build();

        var testCompany = new Company() { CompanyType = CompanyType.Small, Market = Market.Denmark };
        _sut = new CreateNotificationsService(configuration, _loggerMock.Object);


        //act
        var result = _sut.AddNotifications(testCompany);

        //assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Notifications);
        Assert.Equal(testCompany, result);
        Assert.Equal(5, result.Notifications.Count);
        Assert.Equal(DateTime.Now.AddDays(1).ToShortDateString(), result.Notifications[0].SendingDate.ToShortDateString());
        Assert.Equal(DateTime.Now.AddDays(5).ToShortDateString(), result.Notifications[1].SendingDate.ToShortDateString());
        Assert.Equal(DateTime.Now.AddDays(20).ToShortDateString(), result.Notifications[4].SendingDate.ToShortDateString());
    }

    [Fact]
    public void AddNotification_ReturnCompanyWithoutNotifications_WhenCreateNotificationsReturnFalse()
    {
        //arrange
        var testCompany = new Company() { CompanyType = CompanyType.Small, Market = Market.Denmark };
        _sut = new CreateNotificationsService(_configurationMock.Object, _loggerMock.Object);

        //act
        var result = _sut.AddNotifications(testCompany);

        //assert
        Assert.NotNull(result);
        Assert.Empty(result.Notifications);
        Assert.Equal(testCompany, result);
    }

    [Fact]
    public void AddNotification_ReturnCompanyWithoutNotifications_WhenDateTimeInAppsettingsIsOutOfRange()
    {
        //arrange
        var days = (DateTime.MaxValue - DateTime.Now.AddDays(-1)).Days;
        var appSettings = $@"{{""NotificationPeriodicity"": {{
                ""Denmark"": {{
                  ""Small"": [{days}]
                    }}}}}}";

        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(appSettings)));
        var configuration = configurationBuilder.Build();

        var testCompany = new Company() { CompanyType = CompanyType.Small, Market = Market.Denmark };
        _sut = new CreateNotificationsService(configuration, _loggerMock.Object);


        //act
        var result = _sut.AddNotifications(testCompany);

        //assert
        Assert.NotNull(result);
        Assert.Empty(result.Notifications);
        Assert.Equal(testCompany, result);
    }
}