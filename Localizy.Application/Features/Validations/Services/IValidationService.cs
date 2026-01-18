using Localizy.Application.Features.Validations.DTOs;

namespace Localizy.Application.Features.Validations.Services;

public interface IValidationService
{
    Task<ValidationResponseDto?> GetByIdAsync(Guid id);
    Task<ValidationResponseDto?> GetByRequestIdAsync(string requestId);
    Task<IEnumerable<ValidationResponseDto>> GetAllAsync();
    Task<IEnumerable<ValidationResponseDto>> SearchAsync(string searchTerm);
    Task<IEnumerable<ValidationResponseDto>> FilterByStatusAsync(string status);
    Task<IEnumerable<ValidationResponseDto>> FilterByPriorityAsync(string priority);
    Task<IEnumerable<ValidationResponseDto>> GetByUserAsync(Guid userId);
    Task<ValidationStatsDto> GetStatsAsync();
    Task<ValidationResponseDto> CreateAsync(Guid userId, CreateValidationDto dto);
    Task<ValidationResponseDto?> UpdateAsync(Guid id, UpdateValidationDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<ValidationResponseDto?> VerifyAsync(Guid id, Guid verifiedByUserId, VerifyValidationDto dto);
    Task<ValidationResponseDto?> RejectAsync(Guid id, Guid rejectedByUserId, RejectValidationDto dto);
    Task<VerificationRequestResponseDto> CreateVerificationRequestAsync(
        Guid userId, 
        CreateVerificationRequestDto dto,
        string? idDocumentFileName,
        string? idDocumentPath,
        string? addressProofFileName,
        string? addressProofPath);
    Task<VerificationRequestResponseDto?> GetVerificationRequestByIdAsync(Guid id);
}