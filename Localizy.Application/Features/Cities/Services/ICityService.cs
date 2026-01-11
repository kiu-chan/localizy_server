using Localizy.Application.Features.Cities.DTOs;

namespace Localizy.Application.Features.Cities.Services;

public interface ICityService
{
    Task<CityResponseDto?> GetByIdAsync(Guid id);
    Task<CityResponseDto?> GetByCodeAsync(string code);
    Task<IEnumerable<CityResponseDto>> GetAllAsync();
    Task<IEnumerable<CityResponseDto>> GetActiveAsync();
    Task<IEnumerable<CityResponseDto>> SearchAsync(string searchTerm);
    Task<IEnumerable<CityResponseDto>> GetByCountryAsync(string country);
    Task<CityStatsDto> GetStatsAsync();
    Task<CityResponseDto> CreateAsync(CreateCityDto dto);
    Task<CityResponseDto?> UpdateAsync(Guid id, UpdateCityDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ToggleActiveAsync(Guid id);
}