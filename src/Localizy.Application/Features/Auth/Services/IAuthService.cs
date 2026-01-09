using Localizy.Application.Features.Auth.DTOs;

namespace Localizy.Application.Features.Auth.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
}