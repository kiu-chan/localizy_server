namespace Localizy.Application.Features.Addresses.DTOs;

public class CreateAddressDto
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    
    // Thay đổi: chỉ nhận CityId hoặc City name
    public Guid? CityId { get; set; }
    public string Country { get; set; } = string.Empty;
    
    public string Type { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    public string? Description { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public string? OpeningHours { get; set; }
}