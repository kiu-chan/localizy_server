namespace Localizy.Application.Features.Addresses.DTOs;

public class AddressResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    
    public CoordinatesDto Coordinates { get; set; } = new();
    
    public string? Description { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public string? OpeningHours { get; set; }
    
    public double Rating { get; set; }
    public int Views { get; set; }
    public int TotalReviews { get; set; }
    
    public SubmitterDto SubmittedBy { get; set; } = new();
    public DateTime SubmittedDate { get; set; }
    
    public VerifierDto? VerifiedBy { get; set; }
    public DateTime? VerifiedDate { get; set; }
    public string? VerificationNotes { get; set; }
    
    public string? RejectionReason { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CoordinatesDto
{
    public double Lat { get; set; }
    public double Lng { get; set; }
}

public class SubmitterDto
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class VerifierDto
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
}