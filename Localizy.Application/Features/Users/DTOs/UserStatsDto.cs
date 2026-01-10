namespace Localizy.Application.Features.Users.DTOs;

public class UserStatsDto
{
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int SuspendedUsers { get; set; }
    public int InactiveUsers { get; set; }
    public int AdminUsers { get; set; }
    public int ValidatorUsers { get; set; }
    public int BusinessUsers { get; set; }
    public int RegularUsers { get; set; }
}