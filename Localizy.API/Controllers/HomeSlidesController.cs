using Localizy.Application.Features.HomeSlides.DTOs;
using Localizy.Application.Features.HomeSlides.Services;
using Localizy.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Localizy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeSlidesController : ControllerBase
{
    private readonly IHomeSlideService _homeSlideService;
    private readonly IFileService _fileService;

    public HomeSlidesController(IHomeSlideService homeSlideService, IFileService fileService)
    {
        _homeSlideService = homeSlideService;
        _fileService = fileService;
    }

    /// <summary>
    /// Get all active home slides (Public)
    /// </summary>
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<HomeSlideResponseDto>>> GetActive()
    {
        var slides = await _homeSlideService.GetActiveAsync();
        return Ok(slides);
    }

    /// <summary>
    /// Get all home slides including inactive (Admin only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<HomeSlideResponseDto>>> GetAll()
    {
        var slides = await _homeSlideService.GetAllAsync();
        return Ok(slides);
    }

    /// <summary>
    /// Get home slide by ID (Admin only)
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<HomeSlideResponseDto>> GetById(Guid id)
    {
        var slide = await _homeSlideService.GetByIdAsync(id);
        
        if (slide == null)
            return NotFound(new { message = "Home slide not found" });

        return Ok(slide);
    }

    /// <summary>
    /// Create new home slide with image upload (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<HomeSlideResponseDto>> Create(
        [FromForm] IFormFile image,
        [FromForm] string content,
        [FromForm] int order = 0,
        [FromForm] bool isActive = true)
    {
        try
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest(new { message = "Image file is required" });
            }

            var (fileName, filePath) = await _fileService.SaveFileAsync(image, "home-slides");
            
            var dto = new CreateHomeSlideDto
            {
                Content = content,
                Order = order,
                IsActive = isActive
            };
            
            var slide = await _homeSlideService.CreateAsync(dto, fileName, filePath);
            
            return CreatedAtAction(nameof(GetById), new { id = slide.Id }, slide);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update home slide (Admin only)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<HomeSlideResponseDto>> Update(
        Guid id,
        [FromForm] IFormFile? image = null,
        [FromForm] string? content = null,
        [FromForm] int? order = null,
        [FromForm] bool? isActive = null)
    {
        try
        {
            string? newFileName = null;
            string? newFilePath = null;

            if (image != null && image.Length > 0)
            {
                var oldImagePath = await _homeSlideService.GetImagePathByIdAsync(id);
                if (oldImagePath != null)
                {
                    await _fileService.DeleteFileAsync(oldImagePath);
                }

                (newFileName, newFilePath) = await _fileService.SaveFileAsync(image, "home-slides");
            }

            var dto = new UpdateHomeSlideDto
            {
                Content = content,
                Order = order,
                IsActive = isActive
            };

            var slide = await _homeSlideService.UpdateAsync(id, dto, newFileName, newFilePath);
            
            if (slide == null)
                return NotFound(new { message = "Home slide not found" });

            return Ok(slide);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete home slide and its image (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var imagePath = await _homeSlideService.GetImagePathByIdAsync(id);
        
        var result = await _homeSlideService.DeleteAsync(id);
        
        if (!result)
            return NotFound(new { message = "Home slide not found" });

        if (imagePath != null)
        {
            await _fileService.DeleteFileAsync(imagePath);
        }

        return NoContent();
    }
}