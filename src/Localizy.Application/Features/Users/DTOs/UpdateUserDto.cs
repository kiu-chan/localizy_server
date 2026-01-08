namespace Localizy.Application.Features.Users.DTOs;

public class UpdateUserDto
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public bool? IsActive { get; set; }
}