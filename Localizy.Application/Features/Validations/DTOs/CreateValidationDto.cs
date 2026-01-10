namespace Localizy.Application.Features.Validations.DTOs;

public class CreateValidationDto
{
    public Guid AddressId { get; set; }
    public string RequestType { get; set; } = "NewAddress"; // NewAddress, UpdateInformation, DeleteRequest
    public string Priority { get; set; } = "Medium"; // Low, Medium, High
    
    public string? Notes { get; set; }
    public string? OldData { get; set; }
    public string? NewData { get; set; }
    
    public bool PhotosProvided { get; set; }
    public bool DocumentsProvided { get; set; }
    public int AttachmentsCount { get; set; }
}