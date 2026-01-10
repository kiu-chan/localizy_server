namespace Localizy.Domain.Entities;

public class Project : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DefaultLanguage { get; set; } = "en";
    
    // Thêm UserId
    public Guid UserId { get; set; }
    
    // Navigation properties
    public User User { get; set; } = null!;
    public ICollection<Translation> Translations { get; set; } = new List<Translation>();
}