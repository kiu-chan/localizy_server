using Localizy.Application.Features.Cities.DTOs;
using Localizy.Application.Features.Cities.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Localizy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitiesController : ControllerBase
{
    private readonly ICityService _cityService;

    public CitiesController(ICityService cityService)
    {
        _cityService = cityService;
    }

    /// <summary>
    /// Lấy thống kê cities (Admin only)
    /// </summary>
    [HttpGet("stats")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CityStatsDto>> GetStats()
    {
        var stats = await _cityService.GetStatsAsync();
        return Ok(stats);
    }

    /// <summary>
    /// Tìm kiếm cities (Public)
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<CityResponseDto>>> Search([FromQuery] string searchTerm)
    {
        var cities = await _cityService.SearchAsync(searchTerm);
        return Ok(cities);
    }

    /// <summary>
    /// Lấy cities theo country (Public)
    /// </summary>
    [HttpGet("country/{country}")]
    public async Task<ActionResult<IEnumerable<CityResponseDto>>> GetByCountry(string country)
    {
        var cities = await _cityService.GetByCountryAsync(country);
        return Ok(cities);
    }

    /// <summary>
    /// Lấy tất cả cities active (Public)
    /// </summary>
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<CityResponseDto>>> GetActive()
    {
        var cities = await _cityService.GetActiveAsync();
        return Ok(cities);
    }

    /// <summary>
    /// Lấy tất cả cities (Public)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityResponseDto>>> GetAll()
    {
        var cities = await _cityService.GetAllAsync();
        return Ok(cities);
    }

    /// <summary>
    /// Lấy city theo ID (Public)
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<CityResponseDto>> GetById(Guid id)
    {
        var city = await _cityService.GetByIdAsync(id);
        
        if (city == null)
            return NotFound(new { message = "Không tìm thấy thành phố" });

        return Ok(city);
    }

    /// <summary>
    /// Lấy city theo code (Public)
    /// </summary>
    [HttpGet("code/{code}")]
    public async Task<ActionResult<CityResponseDto>> GetByCode(string code)
    {
        var city = await _cityService.GetByCodeAsync(code);
        
        if (city == null)
            return NotFound(new { message = "Không tìm thấy thành phố" });

        return Ok(city);
    }

    /// <summary>
    /// Tạo city mới (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CityResponseDto>> Create([FromBody] CreateCityDto dto)
    {
        try
        {
            var city = await _cityService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = city.Id }, city);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Cập nhật city (Admin only)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CityResponseDto>> Update(Guid id, [FromBody] UpdateCityDto dto)
    {
        try
        {
            var city = await _cityService.UpdateAsync(id, dto);
            
            if (city == null)
                return NotFound(new { message = "Không tìm thấy thành phố" });

            return Ok(city);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Xóa city (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _cityService.DeleteAsync(id);
        
        if (!result)
            return NotFound(new { message = "Không tìm thấy thành phố" });

        return NoContent();
    }

    /// <summary>
    /// Toggle active status (Admin only)
    /// </summary>
    [HttpPatch("{id}/toggle-active")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> ToggleActive(Guid id)
    {
        var result = await _cityService.ToggleActiveAsync(id);
        
        if (!result)
            return NotFound(new { message = "Không tìm thấy thành phố" });

        return Ok(new { message = "Đã cập nhật trạng thái thành công" });
    }
}