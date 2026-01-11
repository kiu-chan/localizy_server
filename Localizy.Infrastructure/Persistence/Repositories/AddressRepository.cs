using Localizy.Application.Common.Interfaces;
using Localizy.Domain.Entities;
using Localizy.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Localizy.Infrastructure.Persistence.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly ApplicationDbContext _context;

    public AddressRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Address?> GetByIdAsync(Guid id)
    {
        return await _context.Addresses
            .Include(a => a.SubmittedByUser)
            .Include(a => a.VerifiedByUser)
            .Include(a => a.City)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Address>> GetAllAsync()
    {
        return await _context.Addresses
            .Include(a => a.SubmittedByUser)
            .Include(a => a.VerifiedByUser)
            .Include(a => a.City)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Address>> GetByStatusAsync(AddressStatus status)
    {
        return await _context.Addresses
            .Include(a => a.SubmittedByUser)
            .Include(a => a.VerifiedByUser)
            .Include(a => a.City)
            .Where(a => a.Status == status)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Address>> GetByTypeAsync(string type)
    {
        return await _context.Addresses
            .Include(a => a.SubmittedByUser)
            .Include(a => a.VerifiedByUser)
            .Include(a => a.City)
            .Where(a => a.Type.ToLower() == type.ToLower())
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Address>> GetByUserAsync(Guid userId)
    {
        return await _context.Addresses
            .Include(a => a.SubmittedByUser)
            .Include(a => a.VerifiedByUser)
            .Include(a => a.City)
            .Where(a => a.SubmittedByUserId == userId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Address>> SearchAsync(string searchTerm)
    {
        var lowerSearchTerm = searchTerm.ToLower();
        
        return await _context.Addresses
            .Include(a => a.SubmittedByUser)
            .Include(a => a.VerifiedByUser)
            .Include(a => a.City)
            .Where(a => 
                a.Name.ToLower().Contains(lowerSearchTerm) ||
                a.FullAddress.ToLower().Contains(lowerSearchTerm) ||
                a.Country.ToLower().Contains(lowerSearchTerm) ||
                a.Category.ToLower().Contains(lowerSearchTerm)
            )
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<Address> CreateAsync(Address address)
    {
        _context.Addresses.Add(address);
        await _context.SaveChangesAsync();
        
        // Load navigation properties
        await _context.Entry(address)
            .Reference(a => a.SubmittedByUser)
            .LoadAsync();
        
        if (address.CityId.HasValue)
        {
            await _context.Entry(address)
                .Reference(a => a.City)
                .LoadAsync();
        }
            
        return address;
    }

    public async Task<Address> UpdateAsync(Address address)
    {
        _context.Addresses.Update(address);
        await _context.SaveChangesAsync();
        
        // Reload navigation properties
        await _context.Entry(address)
            .Reference(a => a.SubmittedByUser)
            .LoadAsync();
        await _context.Entry(address)
            .Reference(a => a.VerifiedByUser)
            .LoadAsync();
        
        if (address.CityId.HasValue)
        {
            await _context.Entry(address)
                .Reference(a => a.City)
                .LoadAsync();
        }
            
        return address;
    }

    public async Task DeleteAsync(Guid id)
    {
        var address = await _context.Addresses.FindAsync(id);
        if (address != null)
        {
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Addresses.AnyAsync(a => a.Id == id);
    }

    public async Task<int> CountByStatusAsync(AddressStatus status)
    {
        return await _context.Addresses.CountAsync(a => a.Status == status);
    }

    public async Task<int> GetTotalViewsAsync()
    {
        return await _context.Addresses.SumAsync(a => a.Views);
    }

    public async Task<double> GetAverageRatingAsync()
    {
        var addresses = await _context.Addresses.Where(a => a.Rating > 0).ToListAsync();
        return addresses.Any() ? addresses.Average(a => a.Rating) : 0;
    }
}