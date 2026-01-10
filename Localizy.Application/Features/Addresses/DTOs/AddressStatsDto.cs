namespace Localizy.Application.Features.Addresses.DTOs;

public class AddressStatsDto
{
    public int TotalAddresses { get; set; }
    public int VerifiedAddresses { get; set; }
    public int PendingAddresses { get; set; }
    public int RejectedAddresses { get; set; }
    public int TotalViews { get; set; }
    public double AverageRating { get; set; }
}