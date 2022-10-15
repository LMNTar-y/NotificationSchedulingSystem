using Moq;
using NotificationSchedulingSystem.Business.Models;
using NotificationSchedulingSystem.Business.Services;

namespace NotificationSchedulingSystem.UnitTests.IntegrationAPITests;

public class CompanyServiceMock : Mock<ICompanyService>
{
    public CompanyServiceMock Setup()
    {
        Setup(x => x.GetScheduleByCompanyId(It.IsAny<Guid>())).ReturnsAsync(() => new CompanyResponse());
        Setup(x => x.AddAsync(It.IsAny<CompanyRequest>())).ReturnsAsync(true);
        Setup(x => x.GetAllAsync()).ReturnsAsync(() => new List<CompanyResponse>());
        return this;
    }
}
