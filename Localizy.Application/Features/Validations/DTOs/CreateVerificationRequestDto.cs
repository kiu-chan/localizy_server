namespace Localizy.Application.Features.Validations.DTOs;

public class CreateVerificationRequestDto
{
    public Guid? AddressId { get; set; }
    public string RequestType { get; set; } = "NewAddress";
    public string Priority { get; set; } = "Medium";
    
    public string IdType { get; set; } = "CMND";
    public string? Notes { get; set; }
    
    public bool PhotosProvided { get; set; }
    public bool DocumentsProvided { get; set; }
    public int AttachmentsCount { get; set; }
    
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal PaymentAmount { get; set; } = 100000;
    
    public DateTime? AppointmentDate { get; set; }
    public string? AppointmentTimeSlot { get; set; }
}