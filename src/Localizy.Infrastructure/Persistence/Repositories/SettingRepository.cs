using Localizy.Application.Common.Interfaces;
using Localizy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Localizy.Infrastructure.Persistence.Repositories;

public class SettingRepository : ISettingRepository
{
    private readonly ApplicationDbContext _context;

    public SettingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Setting?> GetByKeyAsync(string key)
    {
        return await _context.Settings.FirstOrDefaultAsync(s => s.Key == key);
    }

    public async Task<IEnumerable<Setting>> GetAllAsync()
    {
        return await _context.Settings.OrderBy(s => s.Category).ThenBy(s => s.Key).ToListAsync();
    }

    public async Task<IEnumerable<Setting>> GetByCategoryAsync(string category)
    {
        return await _context.Settings
            .Where(s => s.Category == category)
            .OrderBy(s => s.Key)
            .ToListAsync();
    }

    public async Task<Setting> CreateAsync(Setting setting)
    {
        _context.Settings.Add(setting);
        await _context.SaveChangesAsync();
        return setting;
    }

    public async Task<Setting> UpdateAsync(Setting setting)
    {
        _context.Settings.Update(setting);
        await _context.SaveChangesAsync();
        return setting;
    }

    public async Task DeleteAsync(string key)
    {
        var setting = await _context.Settings.FirstOrDefaultAsync(s => s.Key == key);
        if (setting != null)
        {
            _context.Settings.Remove(setting);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsByKeyAsync(string key)
    {
        return await _context.Settings.AnyAsync(s => s.Key == key);
    }
}