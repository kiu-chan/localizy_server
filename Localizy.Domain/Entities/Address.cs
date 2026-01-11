using Localizy.Domain.Enums;

namespace Localizy.Domain.Entities;

public class Address : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string FullAddress { get; set; } = string.Empty;
    
    // City relationship - CHỈ ĐỊNH NGHĨA MỘT LẦN
    public Guid? CityId { get; set; }
    public City? City { get; set; }
    
    public string Country { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public AddressStatus Status { get; set; } = AddressStatus.Pending;
    
    // Location
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    // Details
    public string? Description { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public string? OpeningHours { get; set; }
    
    // Rating & Stats
    public double Rating { get; set; } = 0;
    public int Views { get; set; } = 0;
    public int TotalReviews { get; set; } = 0;
    
    // Submission Info
    public Guid SubmittedByUserId { get; set; }
    public User SubmittedByUser { get; set; } = null!;
    public DateTime SubmittedDate { get; set; }
    
    // Verification Info
    public Guid? VerifiedByUserId { get; set; }
    public User? VerifiedByUser { get; set; }
    public DateTime? VerifiedDate { get; set; }
    public string? VerificationNotes { get; set; }
    
    // Rejection Info
    public string? RejectionReason { get; set; }
}