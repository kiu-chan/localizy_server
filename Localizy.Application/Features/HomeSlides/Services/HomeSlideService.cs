using Localizy.Application.Common.Interfaces;
using Localizy.Application.Features.HomeSlides.DTOs;
using Localizy.Domain.Entities;

namespace Localizy.Application.Features.HomeSlides.Services;

public class HomeSlideService : IHomeSlideService
{
    private readonly IHomeSlideRepository _repository;

    public HomeSlideService(IHomeSlideRepository repository)
    {
        _repository = repository;
    }

    public async Task<HomeSlideResponseDto?> GetByIdAsync(Guid id)
    {
        var slide = await _repository.GetByIdAsync(id);
        return slide == null ? null : MapToDto(slide);
    }

    public async Task<IEnumerable<HomeSlideResponseDto>> GetAllAsync()
    {
        var slides = await _repository.GetAllAsync();
        return slides.Select(MapToDto);
    }

    public async Task<IEnumerable<HomeSlideResponseDto>> GetActiveAsync()
    {
        var slides = await _repository.GetActiveAsync();
        return slides.Select(MapToDto);
    }

    public async Task<HomeSlideResponseDto> CreateAsync(CreateHomeSlideDto dto)
    {
        var slide = new HomeSlide
        {
            Id = Guid.NewGuid(),
            ImageUrl = dto.ImageUrl,
            Content = dto.Content,
            Order = dto.Order,
            IsActive = dto.IsActive
        };

        var createdSlide = await _repository.CreateAsync(slide);
        return MapToDto(createdSlide);
    }

    public async Task<HomeSlideResponseDto?> UpdateAsync(Guid id, UpdateHomeSlideDto dto)
    {
        var slide = await _repository.GetByIdAsync(id);
        if (slide == null) return null;

        if (!string.IsNullOrEmpty(dto.ImageUrl))
            slide.ImageUrl = dto.ImageUrl;

        if (!string.IsNullOrEmpty(dto.Content))
            slide.Content = dto.Content;

        if (dto.Order.HasValue)
            slide.Order = dto.Order.Value;

        if (dto.IsActive.HasValue)
            slide.IsActive = dto.IsActive.Value;

        var updatedSlide = await _repository.UpdateAsync(slide);
        return MapToDto(updatedSlide);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        if (!await _repository.ExistsAsync(id))
            return false;

        await _repository.DeleteAsync(id);
        return true;
    }

    private static HomeSlideResponseDto MapToDto(HomeSlide slide)
    {
        return new HomeSlideResponseDto
        {
            Id = slide.Id,
            ImageUrl = slide.ImageUrl,
            Content = slide.Content,
            Order = slide.Order,
            IsActive = slide.IsActive,
            CreatedAt = slide.CreatedAt,
            UpdatedAt = slide.UpdatedAt
        };
    }
}