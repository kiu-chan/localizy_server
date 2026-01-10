namespace Localizy.Application.Features.Validations.DTOs;

public class ValidationResponseDto
{
    public Guid Id { get; set; }
    public string RequestId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string RequestType { get; set; } = string.Empty;
    
    public ValidationAddressDto Address { get; set; } = new();
    public ValidationSubmitterDto SubmittedBy { get; set; } = new();
    public DateTime SubmittedDate { get; set; }
    
    public string? Notes { get; set; }
    public ValidationChangesDto? Changes { get; set; }
    
    public ValidationVerificationDataDto VerificationData { get; set; } = new();
    public int AttachmentsCount { get; set; }
    
    public ValidationProcessorDto? ProcessedBy { get; set; }
    public DateTime? ProcessedDate { get; set; }
    public string? ProcessingNotes { get; set; }
    public string? RejectionReason { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class ValidationAddressDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public ValidationCoordinatesDto Coordinates { get; set; } = new();
}

public class ValidationCoordinatesDto
{
    public double Lat { get; set; }
    public double Lng { get; set; }
}

public class ValidationSubmitterDto
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class ValidationChangesDto
{
    public string? OldData { get; set; }
    public string? NewData { get; set; }
}

public class ValidationVerificationDataDto
{
    public bool PhotosProvided { get; set; }
    public bool DocumentsProvided { get; set; }
    public bool LocationVerified { get; set; }
}

public class ValidationProcessorDto
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
}