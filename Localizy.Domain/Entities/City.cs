namespace Localizy.Domain.Entities;

public class City : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty; // VN-HN, VN-HCM, VN-DN
    public string Country { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
}