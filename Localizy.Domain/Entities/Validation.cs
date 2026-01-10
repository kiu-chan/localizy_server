using Localizy.Domain.Enums;

namespace Localizy.Domain.Entities;

public class Validation : BaseEntity
{
    public string RequestId { get; set; } = string.Empty; // VAL-2024-001
    public Guid AddressId { get; set; }
    public Address Address { get; set; } = null!;
    
    public ValidationStatus Status { get; set; } = ValidationStatus.Pending;
    public ValidationPriority Priority { get; set; } = ValidationPriority.Medium;
    public ValidationRequestType RequestType { get; set; }
    
    // Submission Info
    public Guid SubmittedByUserId { get; set; }
    public User SubmittedByUser { get; set; } = null!;
    public DateTime SubmittedDate { get; set; }
    
    // Request Details
    public string? Notes { get; set; }
    public string? OldData { get; set; } // JSON string for update requests
    public string? NewData { get; set; } // JSON string for update requests
    
    // Verification Data
    public bool PhotosProvided { get; set; }
    public bool DocumentsProvided { get; set; }
    public bool LocationVerified { get; set; }
    public int AttachmentsCount { get; set; }
    
    // Verification/Rejection Info
    public Guid? ProcessedByUserId { get; set; }
    public User? ProcessedByUser { get; set; }
    public DateTime? ProcessedDate { get; set; }
    public string? ProcessingNotes { get; set; }
    public string? RejectionReason { get; set; }
}