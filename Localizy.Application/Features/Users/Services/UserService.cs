using Localizy.Application.Common.Interfaces;
using Localizy.Application.Features.Users.DTOs;
using Localizy.Domain.Entities;
using Localizy.Domain.Enums;

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

    public async Task<IEnumerable<UserResponseDto>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return await GetAllAsync();
        }

        var users = await _userRepository.SearchAsync(searchTerm);
        return users.Select(MapToDto);
    }

    public async Task<IEnumerable<UserResponseDto>> FilterByRoleAsync(string role)
    {
        if (!Enum.TryParse<UserRole>(role, true, out var userRole))
        {
            return await GetAllAsync();
        }

        var users = await _userRepository.GetByRoleAsync(userRole);
        return users.Select(MapToDto);
    }

    public async Task<IEnumerable<UserResponseDto>> FilterByStatusAsync(bool isActive)
    {
        var users = await _userRepository.GetByStatusAsync(isActive);
        return users.Select(MapToDto);
    }

    public async Task<UserStatsDto> GetStatsAsync()
    {
        var allUsers = await _userRepository.GetAllAsync();
        var usersList = allUsers.ToList();

        return new UserStatsDto
        {
            TotalUsers = usersList.Count,
            ActiveUsers = usersList.Count(u => u.IsActive),
            SuspendedUsers = usersList.Count(u => !u.IsActive && u.LastLoginAt != null),
            InactiveUsers = usersList.Count(u => !u.IsActive && u.LastLoginAt == null),
            AdminUsers = usersList.Count(u => u.Role == UserRole.Admin),
            ValidatorUsers = await _userRepository.CountByRoleAsync(UserRole.Validator),
            BusinessUsers = await _userRepository.CountByRoleAsync(UserRole.Business),
            RegularUsers = usersList.Count(u => u.Role == UserRole.User)
        };
    }

    public async Task<UserResponseDto> CreateAsync(CreateUserDto dto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Email is already in use");
        }

        if (!Enum.TryParse<UserRole>(dto.Role, true, out var userRole))
        {
            userRole = UserRole.User;
        }

        // Validate SubAccount must have ParentBusinessId
        if (userRole == UserRole.SubAccount && !dto.ParentBusinessId.HasValue)
        {
            throw new InvalidOperationException("SubAccount must have a ParentBusinessId");
        }

        // Validate ParentBusinessId exists and is a Business
        if (dto.ParentBusinessId.HasValue)
        {
            var parentBusiness = await _userRepository.GetByIdAsync(dto.ParentBusinessId.Value);
            if (parentBusiness == null || parentBusiness.Role != UserRole.Business)
            {
                throw new InvalidOperationException("Invalid ParentBusinessId");
            }
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            FullName = dto.FullName,
            Phone = dto.Phone,
            Location = dto.Location,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            IsActive = true,
            Role = userRole,
            ParentBusinessId = userRole == UserRole.SubAccount ? dto.ParentBusinessId : null
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
                throw new InvalidOperationException("Email is already in use");
            
            user.Email = dto.Email;
        }

        if (dto.Phone != null)
            user.Phone = dto.Phone;

        if (dto.Location != null)
            user.Location = dto.Location;

        if (dto.IsActive.HasValue)
            user.IsActive = dto.IsActive.Value;

        if (!string.IsNullOrEmpty(dto.Role))
        {
            if (Enum.TryParse<UserRole>(dto.Role, true, out var userRole))
            {
                user.Role = userRole;
            }
        }

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

    public async Task<bool> ToggleStatusAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return false;

        user.IsActive = !user.IsActive;
        await _userRepository.UpdateAsync(user);
        return true;
    }

    public async Task<bool> ChangePasswordAsync(Guid id, ChangePasswordDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return false;

        if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Current password is incorrect");
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        await _userRepository.UpdateAsync(user);
        return true;
    }

    public async Task<UserResponseDto> CreateSubAccountAsync(Guid parentBusinessId, CreateUserDto dto)
    {
        var parentBusiness = await _userRepository.GetByIdAsync(parentBusinessId);
        if (parentBusiness == null || parentBusiness.Role != UserRole.Business)
        {
            throw new InvalidOperationException("Only Business accounts can create sub-accounts");
        }

        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Email is already in use");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            FullName = dto.FullName,
            Phone = dto.Phone,
            Location = dto.Location,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            IsActive = true,
            Role = UserRole.SubAccount,
            ParentBusinessId = parentBusinessId
        };

        var createdUser = await _userRepository.CreateAsync(user);
        return MapToDto(createdUser);
    }

    public async Task<IEnumerable<UserResponseDto>> GetSubAccountsAsync(Guid parentBusinessId)
    {
        var subAccounts = await _userRepository.GetSubAccountsByParentAsync(parentBusinessId);
        return subAccounts.Select(MapToDto);
    }

    private static UserResponseDto MapToDto(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Phone = user.Phone,
            Location = user.Location,
            Avatar = user.Avatar,
            IsActive = user.IsActive,
            Role = user.Role.ToString(),
            LastLoginAt = user.LastLoginAt,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            TotalAddresses = user.TotalAddresses,
            VerifiedAddresses = user.VerifiedAddresses,
            ParentBusinessId = user.ParentBusinessId,
            ParentBusinessName = user.ParentBusiness?.FullName,
            SubAccountsCount = user.SubAccounts?.Count ?? 0
        };
    }
}