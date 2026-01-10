using Localizy.Application.Features.Settings.DTOs;
using Localizy.Application.Features.Settings.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Localizy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SettingsController : ControllerBase
{
    private readonly ISettingService _settingService;

    public SettingsController(ISettingService settingService)
    {
        _settingService = settingService;
    }

    /// <summary>
    /// Lấy toàn bộ cấu hình website (Public - không cần token)
    /// </summary>
    [HttpGet("website-config")]
    [AllowAnonymous]
    public async Task<ActionResult<WebsiteConfigDto>> GetWebsiteConfig()
    {
        var config = await _settingService.GetWebsiteConfigAsync();
        return Ok(config);
    }

    /// <summary>
    /// Lấy tất cả settings (Chỉ Admin)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<SettingDto>>> GetAll()
    {
        var settings = await _settingService.GetAllAsync();
        return Ok(settings);
    }

    /// <summary>
    /// Lấy settings theo category (Chỉ Admin)
    /// </summary>
    [HttpGet("category/{category}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<SettingDto>>> GetByCategory(string category)
    {
        var settings = await _settingService.GetByCategoryAsync(category);
        return Ok(settings);
    }

    /// <summary>
    /// Lấy setting theo key (Chỉ Admin)
    /// </summary>
    [HttpGet("{key}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SettingDto>> GetByKey(string key)
    {
        var setting = await _settingService.GetByKeyAsync(key);
        
        if (setting == null)
            return NotFound(new { message = $"Setting với key '{key}' không tồn tại" });

        return Ok(setting);
    }

    /// <summary>
    /// Cập nhật setting (Chỉ Admin)
    /// </summary>
    [HttpPut("{key}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SettingDto>> Update(string key, [FromBody] UpdateSettingDto dto)
    {
        try
        {
            var setting = await _settingService.UpdateAsync(key, dto);
            return Ok(setting);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}