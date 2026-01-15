namespace Localizy.Application.Features.Validations.DTOs;

public class CreateVerificationRequestDto
{
    public Guid AddressId { get; set; }
    public string RequestType { get; set; } = "NewAddress";
    public string Priority { get; set; } = "Medium";
    
    // Document information
    public string IdType { get; set; } = "CMND"; // CMND, CCCD, Passport
    public string? Notes { get; set; }
    
    // Verification data
    public bool PhotosProvided { get; set; }
    public bool DocumentsProvided { get; set; }
    public int AttachmentsCount { get; set; }
    
    // Location data
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    // Payment information
    public string PaymentMethod { get; set; } = string.Empty; // momo, zalopay, bank, card
    public decimal PaymentAmount { get; set; } = 100000; // Default verification fee
    
    // Appointment information
    public DateTime? AppointmentDate { get; set; }
    public string? AppointmentTimeSlot { get; set; } // morning, afternoon, evening
}