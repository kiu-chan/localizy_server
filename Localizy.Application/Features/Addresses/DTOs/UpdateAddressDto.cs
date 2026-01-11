namespace Localizy.Application.Features.Addresses.DTOs;

public class UpdateAddressDto
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    
    // Thay đổi: cho phép update CityId
    public Guid? CityId { get; set; }
    public string? Country { get; set; }
    
    public string? Type { get; set; }
    public string? Category { get; set; }
    
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    
    public string? Description { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public string? OpeningHours { get; set; }
}