using Localizy.Domain.Entities;

namespace Localizy.Application.Common.Interfaces;

public interface IHomeSlideRepository
{
    Task<HomeSlide?> GetByIdAsync(Guid id);
    Task<IEnumerable<HomeSlide>> GetAllAsync();
    Task<IEnumerable<HomeSlide>> GetActiveAsync();
    Task<HomeSlide> CreateAsync(HomeSlide slide);
    Task<HomeSlide> UpdateAsync(HomeSlide slide);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}