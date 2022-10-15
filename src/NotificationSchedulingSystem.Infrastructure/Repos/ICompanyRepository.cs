using NotificationSchedulingSystem.Infrastructure.Models;

namespace NotificationSchedulingSystem.Infrastructure.Repos;

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetAllAsync();
    Task<Company> GetByIdAsync(Guid id);
    Task<bool> AddAsync(Company company);
}