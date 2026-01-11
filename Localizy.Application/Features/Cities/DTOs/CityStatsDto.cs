namespace Localizy.Application.Features.Cities.DTOs;

public class CityStatsDto
{
    public int TotalCities { get; set; }
    public int ActiveCities { get; set; }
    public int InactiveCities { get; set; }
    public int TotalAddresses { get; set; }
    public List<CityWithCountDto> TopCities { get; set; } = new();
}

public class CityWithCountDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int AddressCount { get; set; }
}