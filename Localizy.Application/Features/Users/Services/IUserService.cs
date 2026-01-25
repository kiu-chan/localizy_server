using Localizy.Application.Features.Users.DTOs;

namespace Localizy.Application.Features.Users.Services;

public interface IUserService
{
    Task<UserResponseDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<UserResponseDto>> GetAllAsync();
    Task<IEnumerable<UserResponseDto>> SearchAsync(string searchTerm);
    Task<IEnumerable<UserResponseDto>> FilterByRoleAsync(string role);
    Task<IEnumerable<UserResponseDto>> FilterByStatusAsync(bool isActive);
    Task<UserStatsDto> GetStatsAsync();
    Task<UserResponseDto> CreateAsync(CreateUserDto dto);
    Task<UserResponseDto?> UpdateAsync(Guid id, UpdateUserDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ToggleStatusAsync(Guid id);
    Task<bool> ChangePasswordAsync(Guid id, ChangePasswordDto dto, bool skipCurrentPasswordCheck = false);
    Task<UserResponseDto> CreateSubAccountAsync(Guid parentBusinessId, CreateUserDto dto);
    Task<IEnumerable<UserResponseDto>> GetSubAccountsAsync(Guid parentBusinessId);
}