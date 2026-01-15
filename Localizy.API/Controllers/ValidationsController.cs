using Localizy.Application.Features.Validations.DTOs;
using Localizy.Application.Features.Validations.Services;
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

    public ValidationsController(IValidationService validationService)
    {
        _validationService = validationService;
    }

    /// <summary>
    /// Lấy thống kê validations (Admin only)
    /// </summary>
    [HttpGet("stats")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ValidationStatsDto>> GetStats()
    {
        var stats = await _validationService.GetStatsAsync();
        return Ok(stats);
    }

    /// <summary>
    /// Tìm kiếm validations
    /// </summary>
    [HttpGet("search")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<ValidationResponseDto>>> Search([FromQuery] string searchTerm)
    {
        var validations = await _validationService.SearchAsync(searchTerm);
        return Ok(validations);
    }

    /// <summary>
    /// Lọc validations theo status
    /// </summary>
    [HttpGet("filter/status/{status}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<ValidationResponseDto>>> FilterByStatus(string status)
    {
        var validations = await _validationService.FilterByStatusAsync(status);
        return Ok(validations);
    }

    /// <summary>
    /// Lọc validations theo priority
    /// </summary>
    [HttpGet("filter/priority/{priority}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<ValidationResponseDto>>> FilterByPriority(string priority)
    {
        var validations = await _validationService.FilterByPriorityAsync(priority);
        return Ok(validations);
    }

    /// <summary>
    /// Lấy validations của user cụ thể
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<ValidationResponseDto>>> GetByUser(Guid userId)
    {
        var validations = await _validationService.GetByUserAsync(userId);
        return Ok(validations);
    }

    /// <summary>
    /// Lấy validations của user hiện tại
    /// </summary>
    [HttpGet("my-validations")]
    public async Task<ActionResult<IEnumerable<ValidationResponseDto>>> GetMyValidations()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "User không hợp lệ" });
        }

        var validations = await _validationService.GetByUserAsync(userId);
        return Ok(validations);
    }

    /// <summary>
    /// Lấy tất cả validations (Admin only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<ValidationResponseDto>>> GetAll()
    {
        var validations = await _validationService.GetAllAsync();
        return Ok(validations);
    }

    /// <summary>
    /// Lấy validation theo ID
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ValidationResponseDto>> GetById(Guid id)
    {
        var validation = await _validationService.GetByIdAsync(id);

        if (validation == null)
            return NotFound(new { message = "Không tìm thấy validation request" });

        return Ok(validation);
    }

    /// <summary>
    /// Lấy validation theo Request ID
    /// </summary>
    [HttpGet("request/{requestId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ValidationResponseDto>> GetByRequestId(string requestId)
    {
        var validation = await _validationService.GetByRequestIdAsync(requestId);

        if (validation == null)
            return NotFound(new { message = "Không tìm thấy validation request" });

        return Ok(validation);
    }

    /// <summary>
    /// Tạo validation request mới
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ValidationResponseDto>> Create([FromBody] CreateValidationDto dto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "User không hợp lệ" });
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
    /// Cập nhật validation request
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ValidationResponseDto>> Update(Guid id, [FromBody] UpdateValidationDto dto)
    {
        try
        {
            var validation = await _validationService.UpdateAsync(id, dto);

            if (validation == null)
                return NotFound(new { message = "Không tìm thấy validation request" });

            return Ok(validation);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Xóa validation request (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _validationService.DeleteAsync(id);

        if (!result)
            return NotFound(new { message = "Không tìm thấy validation request" });

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
            return Unauthorized(new { message = "User không hợp lệ" });
        }

        var validation = await _validationService.VerifyAsync(id, userId, dto);

        if (validation == null)
            return NotFound(new { message = "Không tìm thấy validation request" });

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
            return BadRequest(new { message = "Lý do từ chối không được để trống" });
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "User không hợp lệ" });
        }

        var validation = await _validationService.RejectAsync(id, userId, dto);

        if (validation == null)
            return NotFound(new { message = "Không tìm thấy validation request" });

        return Ok(validation);
    }

    /// <summary>
    /// Create verification request for address
    /// </summary>
    [HttpPost("verification-request")]
    public async Task<ActionResult<VerificationRequestResponseDto>> CreateVerificationRequest(
        [FromBody] CreateVerificationRequestDto dto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid user" });
            }

            var verificationRequest = await _validationService.CreateVerificationRequestAsync(userId, dto);
            return CreatedAtAction(
                nameof(GetById),
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
        var validation = await _validationService.GetByIdAsync(id);

        if (validation == null)
            return NotFound(new { message = "Verification request not found" });

        // Map to VerificationRequestResponseDto
        // You can create a separate method in service for this
        return Ok(validation);
    }
}