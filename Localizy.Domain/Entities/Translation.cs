namespace Localizy.Domain.Entities;

public class Translation : BaseEntity
{
    public Guid ProjectId { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    
    // Navigation property
    public Project Project { get; set; } = null!;
}