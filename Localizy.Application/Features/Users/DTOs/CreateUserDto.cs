namespace Localizy.Application.Features.Users.DTOs;

public class CreateUserDto
{
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Location { get; set; }
    public string Role { get; set; } = "User"; // User, Admin, Validator, Business, SubAccount
    public Guid? ParentBusinessId { get; set; } // Required for SubAccount
}