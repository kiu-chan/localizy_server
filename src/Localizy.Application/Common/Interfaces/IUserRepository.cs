using Localizy.Domain.Entities;
using Localizy.Domain.Enums;

namespace Localizy.Application.Common.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllAsync();
    Task<IEnumerable<User>> GetByRoleAsync(UserRole role);
    Task<IEnumerable<User>> GetByStatusAsync(bool isActive);
    Task<IEnumerable<User>> SearchAsync(string searchTerm);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<int> CountByRoleAsync(UserRole role);
    Task<int> CountByStatusAsync(bool isActive);
}