using Localizy.Application.Features.Addresses.DTOs;

namespace Localizy.Application.Features.Addresses.Services;

public interface IAddressService
{
    Task<AddressResponseDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<AddressResponseDto>> GetAllAsync();
    Task<IEnumerable<AddressResponseDto>> SearchAsync(string searchTerm);
    Task<IEnumerable<AddressResponseDto>> FilterByStatusAsync(string status);
    Task<IEnumerable<AddressResponseDto>> FilterByTypeAsync(string type);
    Task<IEnumerable<AddressResponseDto>> GetByUserAsync(Guid userId);
    Task<AddressStatsDto> GetStatsAsync();
    Task<AddressResponseDto> CreateAsync(Guid userId, CreateAddressDto dto);
    Task<AddressResponseDto?> UpdateAsync(Guid id, UpdateAddressDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<AddressResponseDto?> VerifyAsync(Guid id, Guid verifiedByUserId, VerifyAddressDto dto);
    Task<AddressResponseDto?> RejectAsync(Guid id, Guid rejectedByUserId, RejectAddressDto dto);
    Task IncrementViewsAsync(Guid id);
    Task<IEnumerable<AddressCoordinateDto>> GetAllCoordinatesAsync();
    Task<AddressResponseDto?> GetDetailByIdAsync(Guid id);
}