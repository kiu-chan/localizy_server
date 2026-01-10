namespace Localizy.Application.Features.Validations.DTOs;

public class UpdateValidationDto
{
    public string? Priority { get; set; }
    public string? Notes { get; set; }
    public bool? PhotosProvided { get; set; }
    public bool? DocumentsProvided { get; set; }
    public bool? LocationVerified { get; set; }
}