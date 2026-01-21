namespace Localizy.Application.Features.Addresses.DTOs;

public class AddressSearchResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public CoordinatesDto Coordinates { get; set; } = new();
}