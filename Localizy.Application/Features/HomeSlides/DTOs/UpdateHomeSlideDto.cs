namespace Localizy.Application.Features.HomeSlides.DTOs;

public class UpdateHomeSlideDto
{
    public string? Content { get; set; }
    public int? Order { get; set; }
    public bool? IsActive { get; set; }
    // Image sẽ được upload qua IFormFile trong controller (optional)
}