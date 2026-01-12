using Localizy.Application.Common.Interfaces;
using Localizy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Localizy.Infrastructure.Persistence.Repositories;

public class HomeSlideRepository : IHomeSlideRepository
{
    private readonly ApplicationDbContext _context;

    public HomeSlideRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<HomeSlide?> GetByIdAsync(Guid id)
    {
        return await _context.HomeSlides.FindAsync(id);
    }

    public async Task<IEnumerable<HomeSlide>> GetAllAsync()
    {
        return await _context.HomeSlides
            .OrderBy(s => s.Order)
            .ToListAsync();
    }

    public async Task<IEnumerable<HomeSlide>> GetActiveAsync()
    {
        return await _context.HomeSlides
            .Where(s => s.IsActive)
            .OrderBy(s => s.Order)
            .ToListAsync();
    }

    public async Task<HomeSlide> CreateAsync(HomeSlide slide)
    {
        _context.HomeSlides.Add(slide);
        await _context.SaveChangesAsync();
        return slide;
    }

    public async Task<HomeSlide> UpdateAsync(HomeSlide slide)
    {
        _context.HomeSlides.Update(slide);
        await _context.SaveChangesAsync();
        return slide;
    }

    public async Task DeleteAsync(Guid id)
    {
        var slide = await _context.HomeSlides.FindAsync(id);
        if (slide != null)
        {
            _context.HomeSlides.Remove(slide);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.HomeSlides.AnyAsync(s => s.Id == id);
    }
}