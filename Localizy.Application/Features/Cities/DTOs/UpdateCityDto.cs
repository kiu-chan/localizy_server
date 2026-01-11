namespace Localizy.Application.Features.Cities.DTOs;

public class UpdateCityDto
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Country { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}