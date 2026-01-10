using Localizy.Application.Features.Addresses.DTOs;
using Localizy.Application.Features.Addresses.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Localizy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressesController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressesController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    /// <summary>
    /// Lấy thống kê addresses
    /// </summary>
    [HttpGet("stats")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AddressStatsDto>> GetStats()
    {
        var stats = await _addressService.GetStatsAsync();
        return Ok(stats);
    }

    /// <summary>
    /// Tìm kiếm addresses
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<AddressResponseDto>>> Search([FromQuery] string searchTerm)
    {
        var addresses = await _addressService.SearchAsync(searchTerm);
        return Ok(addresses);
    }

    /// <summary>
    /// Lọc addresses theo status
    /// </summary>
    [HttpGet("filter/status/{status}")]
    public async Task<ActionResult<IEnumerable<AddressResponseDto>>> FilterByStatus(string status)
    {
        var addresses = await _addressService.FilterByStatusAsync(status);
        return Ok(addresses);
    }

    /// <summary>
    /// Lọc addresses theo type
    /// </summary>
    [HttpGet("filter/type/{type}")]
    public async Task<ActionResult<IEnumerable<AddressResponseDto>>> FilterByType(string type)
    {
        var addresses = await _addressService.FilterByTypeAsync(type);
        return Ok(addresses);
    }

    /// <summary>
    /// Lấy addresses của user
    /// </summary>
    [HttpGet("user/{userId}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<AddressResponseDto>>> GetByUser(Guid userId)
    {
        var addresses = await _addressService.GetByUserAsync(userId);
        return Ok(addresses);
    }

    /// <summary>
    /// Lấy addresses của user hiện tại
    /// </summary>
    [HttpGet("my-addresses")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<AddressResponseDto>>> GetMyAddresses()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "User không hợp lệ" });
        }

        var addresses = await _addressService.GetByUserAsync(userId);
        return Ok(addresses);
    }

    /// <summary>
    /// Lấy tất cả addresses
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AddressResponseDto>>> GetAll()
    {
        var addresses = await _addressService.GetAllAsync();
        return Ok(addresses);
    }

    /// <summary>
    /// Lấy address theo ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<AddressResponseDto>> GetById(Guid id)
    {
        var address = await _addressService.GetByIdAsync(id);
        
        if (address == null)
            return NotFound(new { message = "Không tìm thấy address" });

        // Increment views
        await _addressService.IncrementViewsAsync(id);

        return Ok(address);
    }

    /// <summary>
    /// Tạo address mới
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<AddressResponseDto>> Create([FromBody] CreateAddressDto dto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "User không hợp lệ" });
            }

            var address = await _addressService.CreateAsync(userId, dto);
            return CreatedAtAction(nameof(GetById), new { id = address.Id }, address);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Cập nhật address
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<AddressResponseDto>> Update(Guid id, [FromBody] UpdateAddressDto dto)
    {
        try
        {
            var address = await _addressService.UpdateAsync(id, dto);
            
            if (address == null)
                return NotFound(new { message = "Không tìm thấy address" });

            return Ok(address);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Xóa address
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _addressService.DeleteAsync(id);
        
        if (!result)
            return NotFound(new { message = "Không tìm thấy address" });

        return NoContent();
    }

    /// <summary>
    /// Verify address (Admin only)
    /// </summary>
    [HttpPost("{id}/verify")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AddressResponseDto>> Verify(Guid id, [FromBody] VerifyAddressDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "User không hợp lệ" });
        }

        var address = await _addressService.VerifyAsync(id, userId, dto);
        
        if (address == null)
            return NotFound(new { message = "Không tìm thấy address" });

        return Ok(address);
    }

    /// <summary>
    /// Reject address (Admin only)
    /// </summary>
    [HttpPost("{id}/reject")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AddressResponseDto>> Reject(Guid id, [FromBody] RejectAddressDto dto)
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

        var address = await _addressService.RejectAsync(id, userId, dto);
        
        if (address == null)
            return NotFound(new { message = "Không tìm thấy address" });

        return Ok(address);
    }
}