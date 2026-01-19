using Localizy.Application.Common.Interfaces;
using Localizy.Application.Features.Addresses.DTOs;
using Localizy.Domain.Entities;
using Localizy.Domain.Enums;

namespace Localizy.Application.Features.Addresses.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUserRepository _userRepository;

    public AddressService(IAddressRepository addressRepository, IUserRepository userRepository)
    {
        _addressRepository = addressRepository;
        _userRepository = userRepository;
    }

    public async Task<AddressResponseDto?> GetByIdAsync(Guid id)
    {
        var address = await _addressRepository.GetByIdAsync(id);
        return address == null ? null : MapToDto(address);
    }

    public async Task<IEnumerable<AddressResponseDto>> GetAllAsync()
    {
        var addresses = await _addressRepository.GetAllAsync();
        return addresses.Select(MapToDto);
    }

    public async Task<IEnumerable<AddressResponseDto>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return await GetAllAsync();
        }

        var addresses = await _addressRepository.SearchAsync(searchTerm);
        return addresses.Select(MapToDto);
    }

    public async Task<IEnumerable<AddressResponseDto>> FilterByStatusAsync(string status)
    {
        if (!Enum.TryParse<AddressStatus>(status, true, out var addressStatus))
        {
            return await GetAllAsync();
        }

        var addresses = await _addressRepository.GetByStatusAsync(addressStatus);
        return addresses.Select(MapToDto);
    }

    public async Task<IEnumerable<AddressResponseDto>> FilterByTypeAsync(string type)
    {
        var addresses = await _addressRepository.GetByTypeAsync(type);
        return addresses.Select(MapToDto);
    }

    public async Task<IEnumerable<AddressResponseDto>> GetByUserAsync(Guid userId)
    {
        var addresses = await _addressRepository.GetByUserAsync(userId);
        return addresses.Select(MapToDto);
    }

    public async Task<AddressStatsDto> GetStatsAsync()
    {
        var allAddresses = await _addressRepository.GetAllAsync();
        var addressList = allAddresses.ToList();

        return new AddressStatsDto
        {
            TotalAddresses = addressList.Count,
            VerifiedAddresses = await _addressRepository.CountByStatusAsync(AddressStatus.Verified),
            PendingAddresses = await _addressRepository.CountByStatusAsync(AddressStatus.Pending),
            RejectedAddresses = await _addressRepository.CountByStatusAsync(AddressStatus.Rejected),
            TotalViews = await _addressRepository.GetTotalViewsAsync(),
            AverageRating = await _addressRepository.GetAverageRatingAsync()
        };
    }

    public async Task<AddressResponseDto> CreateAsync(Guid userId, CreateAddressDto dto)
    {
        // Verify user exists
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new InvalidOperationException("User không tồn tại");
        }

        var address = new Address
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            FullAddress = dto.Address,
            CityId = dto.CityId, // Lưu CityId thay vì City string
            Country = dto.Country,
            Type = dto.Type,
            Category = dto.Category,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            Description = dto.Description,
            Phone = dto.Phone,
            Website = dto.Website,
            OpeningHours = dto.OpeningHours,
            Status = AddressStatus.Pending,
            SubmittedByUserId = userId,
            SubmittedDate = DateTime.UtcNow
        };

        var createdAddress = await _addressRepository.CreateAsync(address);
        return MapToDto(createdAddress);
    }

    public async Task<AddressResponseDto?> UpdateAsync(Guid id, UpdateAddressDto dto)
    {
        var address = await _addressRepository.GetByIdAsync(id);
        if (address == null) return null;

        if (!string.IsNullOrEmpty(dto.Name))
            address.Name = dto.Name;

        if (!string.IsNullOrEmpty(dto.Address))
            address.FullAddress = dto.Address;

        if (dto.CityId.HasValue)
            address.CityId = dto.CityId; // Update CityId

        if (!string.IsNullOrEmpty(dto.Country))
            address.Country = dto.Country;

        if (!string.IsNullOrEmpty(dto.Type))
            address.Type = dto.Type;

        if (!string.IsNullOrEmpty(dto.Category))
            address.Category = dto.Category;

        if (dto.Latitude.HasValue)
            address.Latitude = dto.Latitude.Value;

        if (dto.Longitude.HasValue)
            address.Longitude = dto.Longitude.Value;

        if (dto.Description != null)
            address.Description = dto.Description;

        if (dto.Phone != null)
            address.Phone = dto.Phone;

        if (dto.Website != null)
            address.Website = dto.Website;

        if (dto.OpeningHours != null)
            address.OpeningHours = dto.OpeningHours;

        var updatedAddress = await _addressRepository.UpdateAsync(address);
        return MapToDto(updatedAddress);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        if (!await _addressRepository.ExistsAsync(id))
            return false;

        await _addressRepository.DeleteAsync(id);
        return true;
    }

    public async Task<AddressResponseDto?> VerifyAsync(Guid id, Guid verifiedByUserId, VerifyAddressDto dto)
    {
        var address = await _addressRepository.GetByIdAsync(id);
        if (address == null) return null;

        address.Status = AddressStatus.Verified;
        address.VerifiedByUserId = verifiedByUserId;
        address.VerifiedDate = DateTime.UtcNow;
        address.VerificationNotes = dto.Notes;

        var updatedAddress = await _addressRepository.UpdateAsync(address);
        return MapToDto(updatedAddress);
    }

    public async Task<AddressResponseDto?> RejectAsync(Guid id, Guid rejectedByUserId, RejectAddressDto dto)
    {
        var address = await _addressRepository.GetByIdAsync(id);
        if (address == null) return null;

        address.Status = AddressStatus.Rejected;
        address.VerifiedByUserId = rejectedByUserId;
        address.VerifiedDate = DateTime.UtcNow;
        address.RejectionReason = dto.Reason;

        var updatedAddress = await _addressRepository.UpdateAsync(address);
        return MapToDto(updatedAddress);
    }

    public async Task IncrementViewsAsync(Guid id)
    {
        var address = await _addressRepository.GetByIdAsync(id);
        if (address != null)
        {
            address.Views++;
            await _addressRepository.UpdateAsync(address);
        }
    }

    public async Task<IEnumerable<AddressCoordinateDto>> GetAllCoordinatesAsync()
    {
        var coordinates = await _addressRepository.GetAllCoordinatesAsync();
        return coordinates.Select(c => new AddressCoordinateDto
        {
            Id = c.Id,
            Coordinates = new CoordinatesDto
            {
                Lat = c.Latitude,
                Lng = c.Longitude
            }
        });
    }

    public async Task<AddressResponseDto?> GetDetailByIdAsync(Guid id)
    {
        var address = await _addressRepository.GetByIdAsync(id);
        return address == null ? null : MapToDto(address);
    }

    private static AddressResponseDto MapToDto(Address address)
    {
        return new AddressResponseDto
        {
            Id = address.Id,
            Name = address.Name,
            Address = address.FullAddress,
            City = address.City != null ? new AddressCityDto
            {
                Id = address.City.Id,
                Name = address.City.Name,
                Code = address.City.Code
            } : null,
            Country = address.Country,
            Type = address.Type,
            Category = address.Category,
            Status = address.Status.ToString(),
            Coordinates = new CoordinatesDto
            {
                Lat = address.Latitude,
                Lng = address.Longitude
            },
            Description = address.Description,
            Phone = address.Phone,
            Website = address.Website,
            OpeningHours = address.OpeningHours,
            Rating = address.Rating,
            Views = address.Views,
            TotalReviews = address.TotalReviews,
            SubmittedBy = new SubmitterDto
            {
                UserId = address.SubmittedByUserId,
                Name = address.SubmittedByUser.FullName,
                Email = address.SubmittedByUser.Email
            },
            SubmittedDate = address.SubmittedDate,
            VerifiedBy = address.VerifiedByUser != null ? new VerifierDto
            {
                UserId = address.VerifiedByUser.Id,
                Name = address.VerifiedByUser.FullName
            } : null,
            VerifiedDate = address.VerifiedDate,
            VerificationNotes = address.VerificationNotes,
            RejectionReason = address.RejectionReason,
            CreatedAt = address.CreatedAt,
            UpdatedAt = address.UpdatedAt
        };
    }
}