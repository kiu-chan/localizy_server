using Microsoft.AspNetCore.Http;

namespace Localizy.Application.Features.Validations.DTOs;

public class CreateVerificationRequestFormDto
{
    public string? AddressId { get; set; }
    public string RequestType { get; set; } = "NewAddress";
    public string Priority { get; set; } = "Medium";
    public string IdType { get; set; } = "CMND";
    public bool PhotosProvided { get; set; } = false;
    public bool DocumentsProvided { get; set; } = false;
    public int AttachmentsCount { get; set; } = 0;
    public double Latitude { get; set; } = 0;
    public double Longitude { get; set; } = 0;
    public string PaymentMethod { get; set; } = "";
    public decimal PaymentAmount { get; set; } = 100000;
    public DateTime? AppointmentDate { get; set; }
    public string? AppointmentTimeSlot { get; set; }
    public IFormFile? IdDocument { get; set; }
    public IFormFile? AddressProof { get; set; }
}