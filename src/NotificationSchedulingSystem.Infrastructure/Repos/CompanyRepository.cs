using Microsoft.EntityFrameworkCore;
using NotificationSchedulingSystem.Infrastructure.Models;

namespace NotificationSchedulingSystem.Infrastructure.Repos;

public class CompanyRepository : ICompanyRepository
{
    private readonly NotificationSchedulingSystemContext _context;

    public CompanyRepository(NotificationSchedulingSystemContext context)
    {
        _context = context ??
                   throw new ArgumentException(
                       $"{GetType().Name} Initialization failure due to: {nameof(context)}");
    }

    public async Task<IEnumerable<Company>> GetAllAsync()
    {
        return await _context.Companies.AsNoTracking().Include(x => x.Notifications).ToListAsync();
    }

    public async Task<Company> GetByIdAsync(Guid id)
    {
        var company =
            await _context.Companies.Include(x => x.Notifications).FirstOrDefaultAsync(x => x.Id == id) ??
            throw new ArgumentNullException(nameof(id), $"Company with Id = {id} was not found");
        return company;
    }

    public async Task<bool> AddAsync(Company company)
    {
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();
        return true;
    }
}