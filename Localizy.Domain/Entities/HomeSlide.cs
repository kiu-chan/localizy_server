namespace Localizy.Domain.Entities;

public class HomeSlide : BaseEntity
{
    public string ImageUrl { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Order { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}