using Localizy.Application.Features.Validations.DTOs;
using Localizy.Application.Features.Validations.Services;
using Localizy.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Localizy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ValidationsController : ControllerBase
{
    private readonly IValidationService _validationService;
    private readonly IFileService _fileService;

    public ValidationsController(IValidationService validationService, IFileService fileService)
    {
        _validationService = validationService;
        _fileService = fileService;
    }

    /// <summary>
    /// Get validation statistics (Admin only)
    /// </summary>
    [HttpGet("stats")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ValidationStatsDto>> GetStats()
    {
        var stats = await _validationService.GetStatsAsync();
        return Ok(stats);
    }

    /// <summary>
    /// Search validations
    /// </summary>
    [HttpGet("search")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<ValidationResponseDto>>> Search([FromQuery] string searchTerm)
    {
        var validations = await _validationService.SearchAsync(searchTerm);
        return Ok(validations);
    }

    /// <summary>
    /// Filter validations by status
    /// </summary>
    [HttpGet("filter/status/{status}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<ValidationResponseDto>>> FilterByStatus(string status)
    {
        var validations = await _validationService.FilterByStatusAsync(status);
        return Ok(validations);
    }

    /// <summary>
    /// Filter validations by priority
    /// </summary>
    [HttpGet("filter/priority/{priority}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<ValidationResponseDto>>> FilterByPriority(string priority)
    {
        var validations = await _validationService.FilterByPriorityAsync(priority);
        return Ok(validations);
    }

    /// <summary>
    /// Get validations by user ID
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<ValidationResponseDto>>> GetByUser(Guid userId)
    {
        var validations = await _validationService.GetByUserAsync(userId);
        return Ok(validations);
    }

    /// <summary>
    /// Get validations of current user
    /// </summary>
    [HttpGet("my-validations")]
    public async Task<ActionResult<IEnumerable<ValidationResponseDto>>> GetMyValidations()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Invalid user" });
        }

        var validations = await _validationService.GetByUserAsync(userId);
        return Ok(validations);
    }

    /// <summary>
    /// Get all validations (Admin only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<ValidationResponseDto>>> GetAll()
    {
        var validations = await _validationService.GetAllAsync();
        return Ok(validations);
    }

    /// <summary>
    /// Get validation by ID
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ValidationResponseDto>> GetById(Guid id)
    {
        var validation = await _validationService.GetByIdAsync(id);

        if (validation == null)
            return NotFound(new { message = "Validation request not found" });

        return Ok(validation);
    }

    /// <summary>
    /// Get validation by Request ID
    /// </summary>
    [HttpGet("request/{requestId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ValidationResponseDto>> GetByRequestId(string requestId)
    {
        var validation = await _validationService.GetByRequestIdAsync(requestId);

        if (validation == null)
            return NotFound(new { message = "Validation request not found" });

        return Ok(validation);
    }

    /// <summary>
    /// Create validation request
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ValidationResponseDto>> Create([FromBody] CreateValidationDto dto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid user" });
            }

            var validation = await _validationService.CreateAsync(userId, dto);
            return CreatedAtAction(nameof(GetById), new { id = validation.Id }, validation);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update validation request
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ValidationResponseDto>> Update(Guid id, [FromBody] UpdateValidationDto dto)
    {
        try
        {
            var validation = await _validationService.UpdateAsync(id, dto);

            if (validation == null)
                return NotFound(new { message = "Validation request not found" });

            return Ok(validation);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete validation request (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _validationService.DeleteAsync(id);

        if (!result)
            return NotFound(new { message = "Validation request not found" });

        return NoContent();
    }

    /// <summary>
    /// Verify validation request (Admin only)
    /// </summary>
    [HttpPost("{id}/verify")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ValidationResponseDto>> Verify(Guid id, [FromBody] VerifyValidationDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Invalid user" });
        }

        var validation = await _validationService.VerifyAsync(id, userId, dto);

        if (validation == null)
            return NotFound(new { message = "Validation request not found" });

        return Ok(validation);
    }

    /// <summary>
    /// Reject validation request (Admin only)
    /// </summary>
    [HttpPost("{id}/reject")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ValidationResponseDto>> Reject(Guid id, [FromBody] RejectValidationDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Reason))
        {
            return BadRequest(new { message = "Rejection reason is required" });
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Invalid user" });
        }

        var validation = await _validationService.RejectAsync(id, userId, dto);

        if (validation == null)
            return NotFound(new { message = "Validation request not found" });

        return Ok(validation);
    }

    /// <summary>
    /// Create verification request for address with document upload
    /// </summary>
    [HttpPost("verification-request")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<VerificationRequestResponseDto>> CreateVerificationRequest(
        [FromForm] string? addressId,
        [FromForm] string requestType = "NewAddress",
        [FromForm] string priority = "Medium",
        [FromForm] string idType = "CMND",
        [FromForm] bool photosProvided = false,
        [FromForm] bool documentsProvided = false,
        [FromForm] int attachmentsCount = 0,
        [FromForm] double latitude = 0,
        [FromForm] double longitude = 0,
        [FromForm] string paymentMethod = "",
        [FromForm] decimal paymentAmount = 100000,
        [FromForm] DateTime? appointmentDate = null,
        [FromForm] string? appointmentTimeSlot = null,
        [FromForm] IFormFile? idDocument = null,
        [FromForm] IFormFile? addressProof = null)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid user" });
            }

            string? idDocumentFileName = null;
            string? idDocumentPath = null;
            string? addressProofFileName = null;
            string? addressProofPath = null;

            if (idDocument != null && idDocument.Length > 0)
            {
                (idDocumentFileName, idDocumentPath) = await _fileService.SaveFileAsync(idDocument, "verifications");
            }

            if (addressProof != null && addressProof.Length > 0)
            {
                (addressProofFileName, addressProofPath) = await _fileService.SaveFileAsync(addressProof, "verifications");
            }

            var dto = new CreateVerificationRequestDto
            {
                AddressId = string.IsNullOrEmpty(addressId) ? null : Guid.Parse(addressId),
                RequestType = requestType,
                Priority = priority,
                IdType = idType,
                PhotosProvided = photosProvided || idDocument != null,
                DocumentsProvided = documentsProvided || addressProof != null,
                AttachmentsCount = attachmentsCount,
                Latitude = latitude,
                Longitude = longitude,
                PaymentMethod = paymentMethod,
                PaymentAmount = paymentAmount,
                AppointmentDate = appointmentDate,
                AppointmentTimeSlot = appointmentTimeSlot
            };

            var verificationRequest = await _validationService.CreateVerificationRequestAsync(
                userId,
                dto,
                idDocumentFileName,
                idDocumentPath,
                addressProofFileName,
                addressProofPath);

            return CreatedAtAction(
                nameof(GetVerificationRequest),
                new { id = verificationRequest.Id },
                verificationRequest
            );
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get verification request by ID
    /// </summary>
    [HttpGet("verification-request/{id}")]
    public async Task<ActionResult<VerificationRequestResponseDto>> GetVerificationRequest(Guid id)
    {
        var verificationRequest = await _validationService.GetVerificationRequestByIdAsync(id);

        if (verificationRequest == null)
            return NotFound(new { message = "Verification request not found" });

        return Ok(verificationRequest);
    }
}