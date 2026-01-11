using Localizy.Domain.Entities;

namespace Localizy.Application.Common.Interfaces;

public interface ICityRepository
{
    Task<City?> GetByIdAsync(Guid id);
    Task<City?> GetByCodeAsync(string code);
    Task<IEnumerable<City>> GetAllAsync();
    Task<IEnumerable<City>> GetActiveAsync();
    Task<IEnumerable<City>> GetByCountryAsync(string country);
    Task<IEnumerable<City>> SearchAsync(string searchTerm);
    Task<City> CreateAsync(City city);
    Task<City> UpdateAsync(City city);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsByCodeAsync(string code);
    Task<int> CountActiveAsync();
    Task<int> GetTotalAddressesAsync();
}