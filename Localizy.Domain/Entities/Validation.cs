using Localizy.Domain.Enums;

namespace Localizy.Domain.Entities;

public class Validation : BaseEntity
{
    public string RequestId { get; set; } = string.Empty;
    public Guid? AddressId { get; set; }
    public Address? Address { get; set; }

    public ValidationStatus Status { get; set; } = ValidationStatus.Pending;
    public ValidationPriority Priority { get; set; } = ValidationPriority.Medium;
    public ValidationRequestType RequestType { get; set; }

    public Guid SubmittedByUserId { get; set; }
    public User SubmittedByUser { get; set; } = null!;
    public DateTime SubmittedDate { get; set; }

    public string? Notes { get; set; }
    public string? OldData { get; set; }
    public string? NewData { get; set; }

    public bool PhotosProvided { get; set; }
    public bool DocumentsProvided { get; set; }
    public bool LocationVerified { get; set; }
    public int AttachmentsCount { get; set; }

    public Guid? ProcessedByUserId { get; set; }
    public User? ProcessedByUser { get; set; }
    public DateTime? ProcessedDate { get; set; }
    public string? ProcessingNotes { get; set; }
    public string? RejectionReason { get; set; }

    public string? IdType { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? PaymentMethod { get; set; }
    public decimal? PaymentAmount { get; set; }
    public string? PaymentStatus { get; set; }
    public DateTime? AppointmentDate { get; set; }
    public string? AppointmentTimeSlot { get; set; }

    public string? IdDocumentFileName { get; set; }
    public string? IdDocumentPath { get; set; }
    public string? AddressProofFileName { get; set; }
    public string? AddressProofPath { get; set; }
}