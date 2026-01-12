using Localizy.Application.Features.HomeSlides.DTOs;
using Localizy.Application.Features.HomeSlides.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Localizy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeSlidesController : ControllerBase
{
    private readonly IHomeSlideService _slideService;

    public HomeSlidesController(IHomeSlideService slideService)
    {
        _slideService = slideService;
    }

    /// <summary>
    /// Get all active slides (Public)
    /// </summary>
    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<HomeSlideResponseDto>>> GetActive()
    {
        var slides = await _slideService.GetActiveAsync();
        return Ok(slides);
    }

    /// <summary>
    /// Get all slides (Admin only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<HomeSlideResponseDto>>> GetAll()
    {
        var slides = await _slideService.GetAllAsync();
        return Ok(slides);
    }

    /// <summary>
    /// Get slide by ID (Admin only)
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<HomeSlideResponseDto>> GetById(Guid id)
    {
        var slide = await _slideService.GetByIdAsync(id);
        
        if (slide == null)
            return NotFound(new { message = "Slide not found" });

        return Ok(slide);
    }

    /// <summary>
    /// Create new slide (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<HomeSlideResponseDto>> Create([FromBody] CreateHomeSlideDto dto)
    {
        var slide = await _slideService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = slide.Id }, slide);
    }

    /// <summary>
    /// Update slide (Admin only)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<HomeSlideResponseDto>> Update(Guid id, [FromBody] UpdateHomeSlideDto dto)
    {
        var slide = await _slideService.UpdateAsync(id, dto);
        
        if (slide == null)
            return NotFound(new { message = "Slide not found" });

        return Ok(slide);
    }

    /// <summary>
    /// Delete slide (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _slideService.DeleteAsync(id);
        
        if (!result)
            return NotFound(new { message = "Slide not found" });

        return NoContent();
    }
}