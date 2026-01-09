using Localizy.Application.Common.Interfaces;
using Localizy.Application.Features.Users.DTOs;
using Localizy.Domain.Entities;

namespace Localizy.Application.Features.Users.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponseDto?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user == null ? null : MapToDto(user);
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToDto);
    }

    public async Task<UserResponseDto> CreateAsync(CreateUserDto dto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Email đã được sử dụng");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            FullName = dto.FullName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            IsActive = true
        };

        var createdUser = await _userRepository.CreateAsync(user);
        return MapToDto(createdUser);
    }

    public async Task<UserResponseDto?> UpdateAsync(Guid id, UpdateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;

        if (!string.IsNullOrEmpty(dto.FullName))
            user.FullName = dto.FullName;

        if (!string.IsNullOrEmpty(dto.Email))
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null && existingUser.Id != id)
                throw new InvalidOperationException("Email đã được sử dụng");
            
            user.Email = dto.Email;
        }

        if (dto.IsActive.HasValue)
            user.IsActive = dto.IsActive.Value;

        var updatedUser = await _userRepository.UpdateAsync(user);
        return MapToDto(updatedUser);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        if (!await _userRepository.ExistsAsync(id))
            return false;

        await _userRepository.DeleteAsync(id);
        return true;
    }

    private static UserResponseDto MapToDto(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            IsActive = user.IsActive,
            Role = user.Role.ToString(),
            LastLoginAt = user.LastLoginAt,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}