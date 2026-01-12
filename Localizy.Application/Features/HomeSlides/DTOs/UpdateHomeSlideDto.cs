namespace Localizy.Application.Features.HomeSlides.DTOs;

public class UpdateHomeSlideDto
{
    public string? ImageUrl { get; set; }
    public string? Content { get; set; }
    public int? Order { get; set; }
    public bool? IsActive { get; set; }
}