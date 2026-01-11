namespace Localizy.Application.Features.Cities.DTOs;

public class CreateCityDto
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string? Description { get; set; }
}