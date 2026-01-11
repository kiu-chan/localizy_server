using Localizy.Application.Common.Interfaces;
using Localizy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Localizy.Infrastructure.Persistence.Repositories;

public class CityRepository : ICityRepository
{
    private readonly ApplicationDbContext _context;

    public CityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<City?> GetByIdAsync(Guid id)
    {
        return await _context.Cities
            .Include(c => c.Addresses)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<City?> GetByCodeAsync(string code)
    {
        return await _context.Cities
            .Include(c => c.Addresses)
            .FirstOrDefaultAsync(c => c.Code.ToLower() == code.ToLower());
    }

    public async Task<IEnumerable<City>> GetAllAsync()
    {
        return await _context.Cities
            .Include(c => c.Addresses)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<City>> GetActiveAsync()
    {
        return await _context.Cities
            .Include(c => c.Addresses)
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<City>> GetByCountryAsync(string country)
    {
        return await _context.Cities
            .Include(c => c.Addresses)
            .Where(c => c.Country.ToLower() == country.ToLower())
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<City>> SearchAsync(string searchTerm)
    {
        var lowerSearchTerm = searchTerm.ToLower();
        
        return await _context.Cities
            .Include(c => c.Addresses)
            .Where(c => 
                c.Name.ToLower().Contains(lowerSearchTerm) ||
                c.Code.ToLower().Contains(lowerSearchTerm) ||
                c.Country.ToLower().Contains(lowerSearchTerm)
            )
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<City> CreateAsync(City city)
    {
        _context.Cities.Add(city);
        await _context.SaveChangesAsync();
        return city;
    }

    public async Task<City> UpdateAsync(City city)
    {
        _context.Cities.Update(city);
        await _context.SaveChangesAsync();
        
        // Reload navigation properties
        await _context.Entry(city)
            .Collection(c => c.Addresses)
            .LoadAsync();
            
        return city;
    }

    public async Task DeleteAsync(Guid id)
    {
        var city = await _context.Cities.FindAsync(id);
        if (city != null)
        {
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Cities.AnyAsync(c => c.Id == id);
    }

    public async Task<bool> ExistsByCodeAsync(string code)
    {
        return await _context.Cities.AnyAsync(c => c.Code.ToLower() == code.ToLower());
    }

    public async Task<int> CountActiveAsync()
    {
        return await _context.Cities.CountAsync(c => c.IsActive);
    }

    public async Task<int> GetTotalAddressesAsync()
    {
        return await _context.Addresses.CountAsync();
    }
}