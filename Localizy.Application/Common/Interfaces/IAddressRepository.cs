using Localizy.Domain.Entities;
using Localizy.Domain.Enums;

namespace Localizy.Application.Common.Interfaces;

public interface IAddressRepository
{
    Task<Address?> GetByIdAsync(Guid id);
    Task<IEnumerable<Address>> GetAllAsync();
    Task<IEnumerable<Address>> GetByStatusAsync(AddressStatus status);
    Task<IEnumerable<Address>> GetByTypeAsync(string type);
    Task<IEnumerable<Address>> GetByUserAsync(Guid userId);
    Task<IEnumerable<Address>> SearchAsync(string searchTerm);
    Task<Address> CreateAsync(Address address);
    Task<Address> UpdateAsync(Address address);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<int> CountByStatusAsync(AddressStatus status);
    Task<int> GetTotalViewsAsync();
    Task<double> GetAverageRatingAsync();
}