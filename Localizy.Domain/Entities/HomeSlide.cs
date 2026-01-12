namespace Localizy.Domain.Entities;

public class HomeSlide : BaseEntity
{
    public string ImageFileName { get; set; } = string.Empty;  // Tên file ảnh
    public string ImagePath { get; set; } = string.Empty;      // Đường dẫn lưu file
    public string Content { get; set; } = string.Empty;
    public int Order { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}