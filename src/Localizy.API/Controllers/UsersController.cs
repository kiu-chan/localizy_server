using Localizy.Application.Features.Users.DTOs;
using Localizy.Application.Features.Users.Services;
using Microsoft.AspNetCore.Mvc;

namespace Localizy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Lấy danh sách tất cả users
    /// </summary>
    [HttpGet]
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
    /// Tạo user mới
    /// </summary>
    [HttpPost]
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
    /// Xóa user
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _userService.DeleteAsync(id);
        
        if (!result)
            return NotFound(new { message = "Không tìm thấy user" });

        return NoContent();
    }
}