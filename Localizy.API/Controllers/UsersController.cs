using Localizy.Application.Features.Users.DTOs;
using Localizy.Application.Features.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Localizy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Lấy thống kê users
    /// </summary>
    [HttpGet("stats")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserStatsDto>> GetStats()
    {
        var stats = await _userService.GetStatsAsync();
        return Ok(stats);
    }

    /// <summary>
    /// Tìm kiếm users
    /// </summary>
    [HttpGet("search")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> Search([FromQuery] string searchTerm)
    {
        var users = await _userService.SearchAsync(searchTerm);
        return Ok(users);
    }

    /// <summary>
    /// Lọc users theo role
    /// </summary>
    [HttpGet("filter/role/{role}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> FilterByRole(string role)
    {
        var users = await _userService.FilterByRoleAsync(role);
        return Ok(users);
    }

    /// <summary>
    /// Lọc users theo status
    /// </summary>
    [HttpGet("filter/status")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> FilterByStatus([FromQuery] bool isActive)
    {
        var users = await _userService.FilterByStatusAsync(isActive);
        return Ok(users);
    }

    /// <summary>
    /// Lấy danh sách tất cả users (Chỉ Admin)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    /// <summary>
    /// Lấy thông tin user theo ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponseDto>> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        
        if (user == null)
            return NotFound(new { message = "Không tìm thấy user" });

        return Ok(user);
    }

    /// <summary>
    /// Tạo user mới (Chỉ Admin)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserResponseDto>> Create([FromBody] CreateUserDto dto)
    {
        try
        {
            var user = await _userService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Cập nhật thông tin user
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<UserResponseDto>> Update(Guid id, [FromBody] UpdateUserDto dto)
    {
        try
        {
            var user = await _userService.UpdateAsync(id, dto);
            
            if (user == null)
                return NotFound(new { message = "Không tìm thấy user" });

            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Xóa user (Chỉ Admin)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _userService.DeleteAsync(id);
        
        if (!result)
            return NotFound(new { message = "Không tìm thấy user" });

        return NoContent();
    }

    /// <summary>
    /// Toggle trạng thái user (Active/Suspended)
    /// </summary>
    [HttpPatch("{id}/toggle-status")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> ToggleStatus(Guid id)
    {
        var result = await _userService.ToggleStatusAsync(id);
        
        if (!result)
            return NotFound(new { message = "Không tìm thấy user" });

        return Ok(new { message = "Đã cập nhật trạng thái user" });
    }

    /// <summary>
    /// Đổi mật khẩu
    /// </summary>
    [HttpPost("{id}/change-password")]
    public async Task<ActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordDto dto)
    {
        try
        {
            var result = await _userService.ChangePasswordAsync(id, dto);
            
            if (!result)
                return NotFound(new { message = "Không tìm thấy user" });

            return Ok(new { message = "Đã đổi mật khẩu thành công" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}