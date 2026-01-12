namespace Localizy.Application.Features.HomeSlides.DTOs;

public class CreateHomeSlideDto
{
    public string Content { get; set; } = string.Empty;
    public int Order { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    // Image sẽ được upload qua IFormFile trong controller
}