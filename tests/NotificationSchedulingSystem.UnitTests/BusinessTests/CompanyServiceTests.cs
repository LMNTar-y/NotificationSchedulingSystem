using Moq;
using NotificationSchedulingSystem.Business.Models;
using NotificationSchedulingSystem.Business.Services;
using NotificationSchedulingSystem.Infrastructure.Enums;
using NotificationSchedulingSystem.Infrastructure.Models;
using NotificationSchedulingSystem.Infrastructure.Repos;

namespace NotificationSchedulingSystem.UnitTests.BusinessTests;

public class CompanyServiceTests
{
    private CompanyService? _sut;
    private readonly Mock<ICompanyRepository> _companyRepositoryMock = new();
    private readonly Mock<ICreateNotificationsService> _createNotificationsServiceMock = new();

    [Fact]
    public void Test_Constructor_When_DependenciesInitFailure_Result_Exception()
    {
        //Arrange
        Action act = null;

        //Act
        act = new Action(() =>
        {
            new CompanyService (null, null);
        });

        var exception = Record.Exception(act);

        //Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task GetAllAsync_ValidResultReturned()
    {
        //arrange
        List<Company> list = new()
        {
            new Company()
            {
                Id = Guid.NewGuid(),
                Name = "TestName",
                Number = 123456,
                CompanyType = CompanyType.Small,
                Market = Market.Denmark,
                Notifications = new List<Notification>(){new Notification(){SendingDate = DateTime.Now}}
            }
        };

        _companyRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(() => list);
        _sut = new CompanyService(_companyRepositoryMock.Object, _createNotificationsServiceMock.Object);

        //act
        var result = (await _sut.GetAllAsync()).ToList();

        //assert
        Assert.NotEmpty(result);
        Assert.NotEmpty(result[0].Notifications);
        Assert.Equal(list[0].Id, result[0].CompanyId);
        Assert.Equal( list[0].Notifications.Count, result[0].Notifications?.Count);
        Assert.NotSame(list, result);
    }

    [Fact]
    public async Task GetAllAsync_Exception_WhenRepositoryThrowsException()
    {
        //arrange

        _companyRepositoryMock.Setup(x => x.GetAllAsync()).ThrowsAsync(new Exception());
        _sut = new CompanyService(_companyRepositoryMock.Object, _createNotificationsServiceMock.Object);

        //act
        //assert
        await Assert.ThrowsAsync<Exception>(() => _sut.GetAllAsync());
    }

    [Fact]
    public async Task GetScheduleByCompanyId_ValidResultReturned()
    {
        //arrange
        var company = new Company()
            {
                Id = Guid.NewGuid(),
                Name = "TestName",
                Number = 123456,
                CompanyType = CompanyType.Small,
                Market = Market.Denmark,
                Notifications = new List<Notification>(){new Notification(){SendingDate = DateTime.Now}}
            };

        _companyRepositoryMock.Setup(x => x.GetByIdAsync(company.Id)).ReturnsAsync(() => company);
        _sut = new CompanyService(_companyRepositoryMock.Object, _createNotificationsServiceMock.Object);

        //act
        var result = await _sut.GetScheduleByCompanyId(company.Id);

        //assert
        Assert.NotNull(result);
        Assert.NotSame(company, result);
        Assert.Equal(company.Id, result.CompanyId);
        Assert.Equal(company.Notifications.Count, result.Notifications?.Count);
    }

    [Fact]
    public async Task GetScheduleByCompanyId_Exception_WhenRepositoryThrowsException()
    {
        //arrange
        _companyRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception());
        _sut = new CompanyService(_companyRepositoryMock.Object, _createNotificationsServiceMock.Object);

        //act
        //assert
        await Assert.ThrowsAsync<Exception>(() => _sut.GetScheduleByCompanyId(Guid.NewGuid()));
    }

    [Fact]
    public async Task AddAsync_ReturnTrue_WhenValidCompanyRequestProvided()
    {
        //arrange
        var companyRequest = new CompanyRequest()
        {
            Name = "TestName",
            Number = 123456,
            CompanyType = "Small",
            Market = "Denmark"
        };

        _companyRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Company>())).ReturnsAsync(true);
        _createNotificationsServiceMock.Setup(x => x.AddNotifications(It.IsAny<Company>())).Returns(new Company());
        _sut = new CompanyService(_companyRepositoryMock.Object, _createNotificationsServiceMock.Object);

        //act
        var result = await _sut.AddAsync(companyRequest);

        //assert
        Assert.True(result);
    }

    [Fact]
    public async Task AddAsync_ThrowsArgumentException_WhenCompanyRequestCannotBeMapped()
    {
        //arrange
        var companyRequest = new CompanyRequest()
        {
            Name = "TestName",
            Number = 123456,
            CompanyType = "Test",
            Market = "Test"
        };

        _companyRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Company>())).ReturnsAsync(true);
        _createNotificationsServiceMock.Setup(x => x.AddNotifications(It.IsAny<Company>())).Returns(new Company());
        _sut = new CompanyService(_companyRepositoryMock.Object, _createNotificationsServiceMock.Object);

        //act

        //assert
        await Assert.ThrowsAsync<ArgumentException>(() => _sut.AddAsync(companyRequest));
    }
}