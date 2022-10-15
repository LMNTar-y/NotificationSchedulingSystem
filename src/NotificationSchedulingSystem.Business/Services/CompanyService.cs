using System.Globalization;
using NotificationSchedulingSystem.Business.Models;
using NotificationSchedulingSystem.Infrastructure.Enums;
using NotificationSchedulingSystem.Infrastructure.Models;
using NotificationSchedulingSystem.Infrastructure.Repos;

namespace NotificationSchedulingSystem.Business.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ICreateNotificationsService _createNotificationsService;

    public CompanyService(ICompanyRepository companyRepository, ICreateNotificationsService createNotificationsService)
    {
        _companyRepository = companyRepository ??
                             throw new ArgumentException(
                                 $"{GetType().Name} Initialization failure due to: {nameof(companyRepository)}");
        _createNotificationsService = createNotificationsService ??
                                      throw new ArgumentException(
                                          $"{GetType().Name} Initialization failure due to: {nameof(createNotificationsService)}");
    }

    public async Task<IEnumerable<CompanyResponse>> GetAllAsync()
    {
        var companyResponse = (await _companyRepository.GetAllAsync()).Select(Map);
        return companyResponse;
    }

    public async Task<CompanyResponse> GetScheduleByCompanyId(Guid id)
    {
        var company = await _companyRepository.GetByIdAsync(id);
        var companyResponse = Map(company);

        return companyResponse;
    }

    public async Task<bool> AddAsync(CompanyRequest companyRequest)
    {
        var company = Map(companyRequest);
        company = _createNotificationsService.AddNotifications(company);
        return await _companyRepository.AddAsync(company);
    }

    #region mappers

    private CompanyResponse Map(Company company)
    {
        var companyResponse = new CompanyResponse()
        {
            CompanyId = company.Id,
            Notifications = new List<string>()
        };

        foreach (var item in company.Notifications)
            companyResponse.Notifications.Add(item.SendingDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));

        return companyResponse;
    }

    private Company Map(CompanyRequest companyRequest)
    {
        var type = Enum.Parse<CompanyType>(companyRequest.CompanyType, true);
        var market = Enum.Parse<Market>(companyRequest.Market, true);
        var company = new Company()
        {
            Name = companyRequest.Name,
            Number = companyRequest.Number,
            CompanyType = type,
            Market = market
        };

        return company;
    }

    #endregion
}