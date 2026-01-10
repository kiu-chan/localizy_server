namespace Localizy.Application.Features.Users.DTOs;

public class UpdateUserDto
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Location { get; set; }
    public bool? IsActive { get; set; }
    public string? Role { get; set; }
}