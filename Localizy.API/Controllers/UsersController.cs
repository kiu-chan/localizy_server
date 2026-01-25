using System.Security.Claims;
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
    /// Get user statistics (Admin only)
    /// </summary>
    [HttpGet("stats")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserStatsDto>> GetStats()
    {
        var stats = await _userService.GetStatsAsync();
        return Ok(stats);
    }

    /// <summary>
    /// Search users (Admin only)
    /// </summary>
    [HttpGet("search")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> Search([FromQuery] string searchTerm)
    {
        var users = await _userService.SearchAsync(searchTerm);
        return Ok(users);
    }

    /// <summary>
    /// Filter users by role (Admin only)
    /// </summary>
    [HttpGet("filter/role/{role}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> FilterByRole(string role)
    {
        var users = await _userService.FilterByRoleAsync(role);
        return Ok(users);
    }

    /// <summary>
    /// Filter users by status (Admin only)
    /// </summary>
    [HttpGet("filter/status")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> FilterByStatus([FromQuery] bool isActive)
    {
        var users = await _userService.FilterByStatusAsync(isActive);
        return Ok(users);
    }

    /// <summary>
    /// Get all users (Admin only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponseDto>> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
            return NotFound(new { message = "User not found" });

        return Ok(user);
    }

    /// <summary>
    /// Create new user (Admin only)
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
    /// Create sub-account for Business (Business only)
    /// </summary>
    [HttpPost("sub-accounts")]
    [Authorize(Roles = "Business")]
    public async Task<ActionResult<UserResponseDto>> CreateSubAccount([FromBody] CreateUserDto dto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var businessId))
            {
                return Unauthorized(new { message = "Invalid user" });
            }

            var user = await _userService.CreateSubAccountAsync(businessId, dto);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get sub-accounts of current Business (Business only)
    /// </summary>
    [HttpGet("my-sub-accounts")]
    [Authorize(Roles = "Business")]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetMySubAccounts()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var businessId))
        {
            return Unauthorized(new { message = "Invalid user" });
        }

        var subAccounts = await _userService.GetSubAccountsAsync(businessId);
        return Ok(subAccounts);
    }

    /// <summary>
    /// Update user information
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<UserResponseDto>> Update(Guid id, [FromBody] UpdateUserDto dto)
    {
        try
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(currentUserIdClaim) || !Guid.TryParse(currentUserIdClaim, out var currentUserId))
            {
                return Unauthorized(new { message = "Invalid user" });
            }

            // Check permission: Admin can update anyone, User can only update themselves
            if (currentUserRole != "Admin" && currentUserId != id)
            {
                return Forbid();
            }

            // Only Admin can change role and isActive
            if (currentUserRole != "Admin")
            {
                dto.Role = null;
                dto.IsActive = null;
            }

            var user = await _userService.UpdateAsync(id, dto);

            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete user (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _userService.DeleteAsync(id);

        if (!result)
            return NotFound(new { message = "User not found" });

        return NoContent();
    }

    /// <summary>
    /// Toggle user status Active/Suspended (Admin only)
    /// </summary>
    [HttpPatch("{id}/toggle-status")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> ToggleStatus(Guid id)
    {
        var result = await _userService.ToggleStatusAsync(id);

        if (!result)
            return NotFound(new { message = "User not found" });

        return Ok(new { message = "User status updated successfully" });
    }

    /// <summary>
    /// Change password
    /// </summary>
    [HttpPost("{id}/change-password")]
    public async Task<ActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordDto dto)
    {
        try
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(currentUserIdClaim) || !Guid.TryParse(currentUserIdClaim, out var currentUserId))
            {
                return Unauthorized(new { message = "Invalid user" });
            }

            // Check permission: Admin can change anyone's password, User can only change their own
            if (currentUserRole != "Admin" && currentUserId != id)
            {
                return Forbid();
            }

            // Admin can change password without current password
            bool skipCurrentPasswordCheck = currentUserRole == "Admin" && currentUserId != id;

            var result = await _userService.ChangePasswordAsync(id, dto, skipCurrentPasswordCheck);

            if (!result)
                return NotFound(new { message = "User not found" });

            return Ok(new { message = "Password changed successfully" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}