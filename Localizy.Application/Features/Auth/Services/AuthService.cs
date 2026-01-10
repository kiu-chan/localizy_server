using Localizy.Application.Common.Interfaces;
using Localizy.Application.Features.Auth.DTOs;
using Localizy.Domain.Entities;
using Localizy.Domain.Enums;

namespace Localizy.Application.Features.Auth.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public AuthService(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        // Kiểm tra email đã tồn tại
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Email đã được sử dụng");
        }

        // Tạo user mới với role User
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            FullName = dto.FullName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            IsActive = true,
            Role = UserRole.User
        };

        var createdUser = await _userRepository.CreateAsync(user);
        
        // Generate token
        var token = _jwtService.GenerateToken(createdUser);

        return new AuthResponseDto
        {
            Token = token,
            Email = createdUser.Email,
            FullName = createdUser.FullName,
            Role = createdUser.Role.ToString(),
            UserId = createdUser.Id
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        // Tìm user theo email
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Email hoặc mật khẩu không đúng");
        }

        // Kiểm tra password
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Email hoặc mật khẩu không đúng");
        }

        // Kiểm tra user có active không
        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("Tài khoản đã bị vô hiệu hóa");
        }

        // Cập nhật last login
        user.LastLoginAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        // Generate token
        var token = _jwtService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role.ToString(),
            UserId = user.Id
        };
    }
}