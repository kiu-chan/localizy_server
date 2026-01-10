using Localizy.Application.Common.Interfaces;
using Localizy.Application.Features.Validations.DTOs;
using Localizy.Domain.Entities;
using Localizy.Domain.Enums;

namespace Localizy.Application.Features.Validations.Services;

public class ValidationService : IValidationService
{
    private readonly IValidationRepository _validationRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IUserRepository _userRepository;

    public ValidationService(
        IValidationRepository validationRepository,
        IAddressRepository addressRepository,
        IUserRepository userRepository)
    {
        _validationRepository = validationRepository;
        _addressRepository = addressRepository;
        _userRepository = userRepository;
    }

    public async Task<ValidationResponseDto?> GetByIdAsync(Guid id)
    {
        var validation = await _validationRepository.GetByIdAsync(id);
        return validation == null ? null : MapToDto(validation);
    }

    public async Task<ValidationResponseDto?> GetByRequestIdAsync(string requestId)
    {
        var validation = await _validationRepository.GetByRequestIdAsync(requestId);
        return validation == null ? null : MapToDto(validation);
    }

    public async Task<IEnumerable<ValidationResponseDto>> GetAllAsync()
    {
        var validations = await _validationRepository.GetAllAsync();
        return validations.Select(MapToDto);
    }

    public async Task<IEnumerable<ValidationResponseDto>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return await GetAllAsync();
        }

        var validations = await _validationRepository.SearchAsync(searchTerm);
        return validations.Select(MapToDto);
    }

    public async Task<IEnumerable<ValidationResponseDto>> FilterByStatusAsync(string status)
    {
        if (!Enum.TryParse<ValidationStatus>(status, true, out var validationStatus))
        {
            return await GetAllAsync();
        }

        var validations = await _validationRepository.GetByStatusAsync(validationStatus);
        return validations.Select(MapToDto);
    }

    public async Task<IEnumerable<ValidationResponseDto>> FilterByPriorityAsync(string priority)
    {
        if (!Enum.TryParse<ValidationPriority>(priority, true, out var validationPriority))
        {
            return await GetAllAsync();
        }

        var validations = await _validationRepository.GetByPriorityAsync(validationPriority);
        return validations.Select(MapToDto);
    }

    public async Task<IEnumerable<ValidationResponseDto>> GetByUserAsync(Guid userId)
    {
        var validations = await _validationRepository.GetByUserAsync(userId);
        return validations.Select(MapToDto);
    }

    public async Task<ValidationStatsDto> GetStatsAsync()
    {
        var allValidations = await _validationRepository.GetAllAsync();
        var validationsList = allValidations.ToList();

        return new ValidationStatsDto
        {
            TotalRequests = validationsList.Count,
            PendingRequests = await _validationRepository.CountByStatusAsync(ValidationStatus.Pending),
            VerifiedRequests = await _validationRepository.CountByStatusAsync(ValidationStatus.Verified),
            RejectedRequests = await _validationRepository.CountByStatusAsync(ValidationStatus.Rejected),
            HighPriorityRequests = await _validationRepository.CountByPriorityAsync(ValidationPriority.High),
            TodayRequests = await _validationRepository.CountTodayAsync()
        };
    }

    public async Task<ValidationResponseDto> CreateAsync(Guid userId, CreateValidationDto dto)
    {
        // Verify user exists
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new InvalidOperationException("User không tồn tại");
        }

        // Verify address exists
        var address = await _addressRepository.GetByIdAsync(dto.AddressId);
        if (address == null)
        {
            throw new InvalidOperationException("Address không tồn tại");
        }

        // Parse enums
        if (!Enum.TryParse<ValidationRequestType>(dto.RequestType, true, out var requestType))
        {
            requestType = ValidationRequestType.NewAddress;
        }

        if (!Enum.TryParse<ValidationPriority>(dto.Priority, true, out var priority))
        {
            priority = ValidationPriority.Medium;
        }

        // Generate request ID
        var requestId = await _validationRepository.GenerateRequestIdAsync();

        var validation = new Validation
        {
            Id = Guid.NewGuid(),
            RequestId = requestId,
            AddressId = dto.AddressId,
            Status = ValidationStatus.Pending,
            Priority = priority,
            RequestType = requestType,
            SubmittedByUserId = userId,
            SubmittedDate = DateTime.UtcNow,
            Notes = dto.Notes,
            OldData = dto.OldData,
            NewData = dto.NewData,
            PhotosProvided = dto.PhotosProvided,
            DocumentsProvided = dto.DocumentsProvided,
            LocationVerified = false,
            AttachmentsCount = dto.AttachmentsCount
        };

        var createdValidation = await _validationRepository.CreateAsync(validation);
        return MapToDto(createdValidation);
    }

    public async Task<ValidationResponseDto?> UpdateAsync(Guid id, UpdateValidationDto dto)
    {
        var validation = await _validationRepository.GetByIdAsync(id);
        if (validation == null) return null;

        if (!string.IsNullOrEmpty(dto.Priority))
        {
            if (Enum.TryParse<ValidationPriority>(dto.Priority, true, out var priority))
            {
                validation.Priority = priority;
            }
        }

        if (dto.Notes != null)
            validation.Notes = dto.Notes;

        if (dto.PhotosProvided.HasValue)
            validation.PhotosProvided = dto.PhotosProvided.Value;

        if (dto.DocumentsProvided.HasValue)
            validation.DocumentsProvided = dto.DocumentsProvided.Value;

        if (dto.LocationVerified.HasValue)
            validation.LocationVerified = dto.LocationVerified.Value;

        var updatedValidation = await _validationRepository.UpdateAsync(validation);
        return MapToDto(updatedValidation);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        if (!await _validationRepository.ExistsAsync(id))
            return false;

        await _validationRepository.DeleteAsync(id);
        return true;
    }

    public async Task<ValidationResponseDto?> VerifyAsync(Guid id, Guid verifiedByUserId, VerifyValidationDto dto)
    {
        var validation = await _validationRepository.GetByIdAsync(id);
        if (validation == null) return null;

        validation.Status = ValidationStatus.Verified;
        validation.ProcessedByUserId = verifiedByUserId;
        validation.ProcessedDate = DateTime.UtcNow;
        validation.ProcessingNotes = dto.Notes;

        // Also update the address status
        var address = await _addressRepository.GetByIdAsync(validation.AddressId);
        if (address != null)
        {
            address.Status = AddressStatus.Verified;
            address.VerifiedByUserId = verifiedByUserId;
            address.VerifiedDate = DateTime.UtcNow;
            await _addressRepository.UpdateAsync(address);
        }

        var updatedValidation = await _validationRepository.UpdateAsync(validation);
        return MapToDto(updatedValidation);
    }

    public async Task<ValidationResponseDto?> RejectAsync(Guid id, Guid rejectedByUserId, RejectValidationDto dto)
    {
        var validation = await _validationRepository.GetByIdAsync(id);
        if (validation == null) return null;

        validation.Status = ValidationStatus.Rejected;
        validation.ProcessedByUserId = rejectedByUserId;
        validation.ProcessedDate = DateTime.UtcNow;
        validation.RejectionReason = dto.Reason;

        // Also update the address status
        var address = await _addressRepository.GetByIdAsync(validation.AddressId);
        if (address != null)
        {
            address.Status = AddressStatus.Rejected;
            address.VerifiedByUserId = rejectedByUserId;
            address.VerifiedDate = DateTime.UtcNow;
            address.RejectionReason = dto.Reason;
            await _addressRepository.UpdateAsync(address);
        }

        var updatedValidation = await _validationRepository.UpdateAsync(validation);
        return MapToDto(updatedValidation);
    }

    private static ValidationResponseDto MapToDto(Validation validation)
    {
        return new ValidationResponseDto
        {
            Id = validation.Id,
            RequestId = validation.RequestId,
            Status = validation.Status.ToString(),
            Priority = validation.Priority.ToString(),
            RequestType = validation.RequestType.ToString(),
            Address = new ValidationAddressDto
            {
                Id = validation.Address.Id,
                Name = validation.Address.Name,
                Address = validation.Address.FullAddress,
                City = validation.Address.City,
                Country = validation.Address.Country,
                Type = validation.Address.Type,
                Category = validation.Address.Category,
                Coordinates = new ValidationCoordinatesDto
                {
                    Lat = validation.Address.Latitude,
                    Lng = validation.Address.Longitude
                }
            },
            SubmittedBy = new ValidationSubmitterDto
            {
                UserId = validation.SubmittedByUserId,
                Name = validation.SubmittedByUser.FullName,
                Email = validation.SubmittedByUser.Email
            },
            SubmittedDate = validation.SubmittedDate,
            Notes = validation.Notes,
            Changes = !string.IsNullOrEmpty(validation.OldData) || !string.IsNullOrEmpty(validation.NewData)
                ? new ValidationChangesDto
                {
                    OldData = validation.OldData,
                    NewData = validation.NewData
                }
                : null,
            VerificationData = new ValidationVerificationDataDto
            {
                PhotosProvided = validation.PhotosProvided,
                DocumentsProvided = validation.DocumentsProvided,
                LocationVerified = validation.LocationVerified
            },
            AttachmentsCount = validation.AttachmentsCount,
            ProcessedBy = validation.ProcessedByUser != null
                ? new ValidationProcessorDto
                {
                    UserId = validation.ProcessedByUser.Id,
                    Name = validation.ProcessedByUser.FullName
                }
                : null,
            ProcessedDate = validation.ProcessedDate,
            ProcessingNotes = validation.ProcessingNotes,
            RejectionReason = validation.RejectionReason,
            CreatedAt = validation.CreatedAt,
            UpdatedAt = validation.UpdatedAt
        };
    }
}