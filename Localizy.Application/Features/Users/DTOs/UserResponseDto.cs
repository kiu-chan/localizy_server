namespace Localizy.Application.Features.Users.DTOs;

public class UserResponseDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Location { get; set; }
    public string? Avatar { get; set; }
    public bool IsActive { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime? LastLoginAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Statistics
    public int TotalAddresses { get; set; }
    public int VerifiedAddresses { get; set; }
}