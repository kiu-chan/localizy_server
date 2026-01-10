namespace Localizy.Application.Features.Validations.DTOs;

public class ValidationStatsDto
{
    public int TotalRequests { get; set; }
    public int PendingRequests { get; set; }
    public int VerifiedRequests { get; set; }
    public int RejectedRequests { get; set; }
    public int HighPriorityRequests { get; set; }
    public int TodayRequests { get; set; }
}