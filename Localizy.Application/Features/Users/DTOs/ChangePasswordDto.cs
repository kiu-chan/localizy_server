namespace Localizy.Application.Features.Users.DTOs;

public class ChangePasswordDto
{
    public string? CurrentPassword { get; set; } // Optional for Admin
    public string NewPassword { get; set; } = string.Empty;
}