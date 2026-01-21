using Localizy.Domain.Enums;

namespace Localizy.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Location { get; set; }
    public string? Avatar { get; set; }
    public bool IsActive { get; set; } = true;
    public UserRole Role { get; set; } = UserRole.User;
    public DateTime? LastLoginAt { get; set; }
    
    // Parent Business (for SubAccount)
    public Guid? ParentBusinessId { get; set; }
    public User? ParentBusiness { get; set; }
    public ICollection<User> SubAccounts { get; set; } = new List<User>();

    // Statistics
    public int TotalAddresses { get; set; } = 0;
    public int VerifiedAddresses { get; set; } = 0;

    // Navigation properties
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}