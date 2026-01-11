using Localizy.Application.Common.Interfaces;
using Localizy.Application.Features.Cities.DTOs;
using Localizy.Domain.Entities;

namespace Localizy.Application.Features.Cities.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;

    public CityService(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<CityResponseDto?> GetByIdAsync(Guid id)
    {
        var city = await _cityRepository.GetByIdAsync(id);
        return city == null ? null : MapToDto(city);
    }

    public async Task<CityResponseDto?> GetByCodeAsync(string code)
    {
        var city = await _cityRepository.GetByCodeAsync(code);
        return city == null ? null : MapToDto(city);
    }

    public async Task<IEnumerable<CityResponseDto>> GetAllAsync()
    {
        var cities = await _cityRepository.GetAllAsync();
        return cities.Select(MapToDto);
    }

    public async Task<IEnumerable<CityResponseDto>> GetActiveAsync()
    {
        var cities = await _cityRepository.GetActiveAsync();
        return cities.Select(MapToDto);
    }

    public async Task<IEnumerable<CityResponseDto>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return await GetAllAsync();
        }

        var cities = await _cityRepository.SearchAsync(searchTerm);
        return cities.Select(MapToDto);
    }

    public async Task<IEnumerable<CityResponseDto>> GetByCountryAsync(string country)
    {
        var cities = await _cityRepository.GetByCountryAsync(country);
        return cities.Select(MapToDto);
    }

    public async Task<CityStatsDto> GetStatsAsync()
    {
        var allCities = await _cityRepository.GetAllAsync();
        var citiesList = allCities.ToList();

        var topCities = citiesList
            .OrderByDescending(c => c.Addresses.Count)
            .Take(10)
            .Select(c => new CityWithCountDto
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code,
                AddressCount = c.Addresses.Count
            })
            .ToList();

        return new CityStatsDto
        {
            TotalCities = citiesList.Count,
            ActiveCities = await _cityRepository.CountActiveAsync(),
            InactiveCities = citiesList.Count(c => !c.IsActive),
            TotalAddresses = await _cityRepository.GetTotalAddressesAsync(),
            TopCities = topCities
        };
    }

    public async Task<CityResponseDto> CreateAsync(CreateCityDto dto)
    {
        // Check if code already exists
        if (await _cityRepository.ExistsByCodeAsync(dto.Code))
        {
            throw new InvalidOperationException($"Mã thành phố '{dto.Code}' đã tồn tại");
        }

        var city = new City
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Code = dto.Code.ToUpper(),
            Country = dto.Country,
            Description = dto.Description,
            IsActive = true
        };

        var createdCity = await _cityRepository.CreateAsync(city);
        return MapToDto(createdCity);
    }

    public async Task<CityResponseDto?> UpdateAsync(Guid id, UpdateCityDto dto)
    {
        var city = await _cityRepository.GetByIdAsync(id);
        if (city == null) return null;

        if (!string.IsNullOrEmpty(dto.Name))
            city.Name = dto.Name;

        if (!string.IsNullOrEmpty(dto.Code))
        {
            var codeExists = await _cityRepository.ExistsByCodeAsync(dto.Code);
            var existingCity = await _cityRepository.GetByCodeAsync(dto.Code);
            
            if (codeExists && existingCity?.Id != id)
                throw new InvalidOperationException($"Mã thành phố '{dto.Code}' đã được sử dụng");
            
            city.Code = dto.Code.ToUpper();
        }

        if (!string.IsNullOrEmpty(dto.Country))
            city.Country = dto.Country;

        if (dto.Description != null)
            city.Description = dto.Description;

        if (dto.IsActive.HasValue)
            city.IsActive = dto.IsActive.Value;

        var updatedCity = await _cityRepository.UpdateAsync(city);
        return MapToDto(updatedCity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        if (!await _cityRepository.ExistsAsync(id))
            return false;

        await _cityRepository.DeleteAsync(id);
        return true;
    }

    public async Task<bool> ToggleActiveAsync(Guid id)
    {
        var city = await _cityRepository.GetByIdAsync(id);
        if (city == null) return false;

        city.IsActive = !city.IsActive;
        await _cityRepository.UpdateAsync(city);
        return true;
    }

    private static CityResponseDto MapToDto(City city)
    {
        return new CityResponseDto
        {
            Id = city.Id,
            Name = city.Name,
            Code = city.Code,
            Country = city.Country,
            Description = city.Description,
            IsActive = city.IsActive,
            TotalAddresses = city.Addresses.Count,
            CreatedAt = city.CreatedAt,
            UpdatedAt = city.UpdatedAt
        };
    }
}