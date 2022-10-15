using NotificationSchedulingSystem.Business.Models;

namespace NotificationSchedulingSystem.Business.Services;

public interface ICompanyService
{
    Task<IEnumerable<CompanyResponse>> GetAllAsync();
    Task<CompanyResponse> GetScheduleByCompanyId(Guid id);
    Task<bool> AddAsync(CompanyRequest companyRequest);
}