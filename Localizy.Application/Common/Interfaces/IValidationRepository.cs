using Localizy.Domain.Entities;
using Localizy.Domain.Enums;

namespace Localizy.Application.Common.Interfaces;

public interface IValidationRepository
{
    Task<Validation?> GetByIdAsync(Guid id);
    Task<Validation?> GetByRequestIdAsync(string requestId);
    Task<IEnumerable<Validation>> GetAllAsync();
    Task<IEnumerable<Validation>> GetByStatusAsync(ValidationStatus status);
    Task<IEnumerable<Validation>> GetByPriorityAsync(ValidationPriority priority);
    Task<IEnumerable<Validation>> GetByUserAsync(Guid userId);
    Task<IEnumerable<Validation>> SearchAsync(string searchTerm);
    Task<Validation> CreateAsync(Validation validation);
    Task<Validation> UpdateAsync(Validation validation);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<int> CountByStatusAsync(ValidationStatus status);
    Task<int> CountByPriorityAsync(ValidationPriority priority);
    Task<int> CountTodayAsync();
    Task<string> GenerateRequestIdAsync();
}