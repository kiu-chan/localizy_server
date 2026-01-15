namespace Localizy.Application.Features.Validations.DTOs;

public class VerificationRequestResponseDto
{
    public Guid Id { get; set; }
    public string RequestId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    
    public VerificationAddressInfoDto Address { get; set; } = new();
    public VerificationDocumentDto Documents { get; set; } = new();
    public VerificationLocationDto Location { get; set; } = new();
    public VerificationPaymentDto Payment { get; set; } = new();
    public VerificationAppointmentDto? Appointment { get; set; }
    
    public DateTime SubmittedDate { get; set; }
    public string? ProcessingNotes { get; set; }
    
    public DateTime CreatedAt { get; set; }
}

public class VerificationAddressInfoDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
}

public class VerificationDocumentDto
{
    public string IdType { get; set; } = string.Empty;
    public bool PhotosProvided { get; set; }
    public bool DocumentsProvided { get; set; }
    public int AttachmentsCount { get; set; }
}

public class VerificationLocationDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class VerificationPaymentDto
{
    public string Method { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Completed, Failed
}

public class VerificationAppointmentDto
{
    public DateTime Date { get; set; }
    public string TimeSlot { get; set; } = string.Empty;
}