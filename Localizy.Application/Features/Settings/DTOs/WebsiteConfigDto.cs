namespace Localizy.Application.Features.Settings.DTOs;

public class WebsiteConfigDto
{
    public AppDownloadLinks AppDownload { get; set; } = new();
    public SocialMediaLinks SocialMedia { get; set; } = new();
    public ContactInfo Contact { get; set; } = new();
    public GeneralInfo General { get; set; } = new();
}

public class AppDownloadLinks
{
    public string? IosLink { get; set; }
    public string? AndroidLink { get; set; }
}

public class SocialMediaLinks
{
    public string? Facebook { get; set; }
    public string? Twitter { get; set; }
    public string? Instagram { get; set; }
    public string? LinkedIn { get; set; }
    public string? Youtube { get; set; }
}

public class ContactInfo
{
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
}

public class GeneralInfo
{
    public string? Slogan { get; set; }
    public string? Description { get; set; }
    public string? AboutUs { get; set; }
}