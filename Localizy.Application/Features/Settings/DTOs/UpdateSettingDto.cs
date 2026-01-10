namespace Localizy.Application.Features.Settings.DTOs;

public class UpdateSettingDto
{
    public string Value { get; set; } = string.Empty;
    public string? Description { get; set; }
}