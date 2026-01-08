using Localizy.Application.Features.Users.DTOs;

namespace Localizy.Application.Features.Users.Services;

public interface IUserService
{
    Task<UserResponseDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<UserResponseDto>> GetAllAsync();
    Task<UserResponseDto> CreateAsync(CreateUserDto dto);
    Task<UserResponseDto?> UpdateAsync(Guid id, UpdateUserDto dto);
    Task<bool> DeleteAsync(Guid id);
}