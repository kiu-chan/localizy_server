using Localizy.Application.Features.HomeSlides.DTOs;

namespace Localizy.Application.Features.HomeSlides.Services;

public interface IHomeSlideService
{
    Task<HomeSlideResponseDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<HomeSlideResponseDto>> GetAllAsync();
    Task<IEnumerable<HomeSlideResponseDto>> GetActiveAsync();
    Task<HomeSlideResponseDto> CreateAsync(CreateHomeSlideDto dto);
    Task<HomeSlideResponseDto?> UpdateAsync(Guid id, UpdateHomeSlideDto dto);
    Task<bool> DeleteAsync(Guid id);
}