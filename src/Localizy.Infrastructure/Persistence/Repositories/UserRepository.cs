using Localizy.Application.Common.Interfaces;
using Localizy.Domain.Entities;
using Localizy.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Localizy.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .Include(u => u.Projects)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users
            .Include(u => u.Projects)
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetByRoleAsync(UserRole role)
    {
        return await _context.Users
            .Where(u => u.Role == role)
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetByStatusAsync(bool isActive)
    {
        return await _context.Users
            .Where(u => u.IsActive == isActive)
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> SearchAsync(string searchTerm)
    {
        var lowerSearchTerm = searchTerm.ToLower();
        
        return await _context.Users
            .Where(u => 
                u.FullName.ToLower().Contains(lowerSearchTerm) ||
                u.Email.ToLower().Contains(lowerSearchTerm) ||
                (u.Phone != null && u.Phone.Contains(searchTerm)) ||
                (u.Location != null && u.Location.ToLower().Contains(lowerSearchTerm))
            )
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync();
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Users.AnyAsync(u => u.Id == id);
    }

    public async Task<int> CountByRoleAsync(UserRole role)
    {
        return await _context.Users.CountAsync(u => u.Role == role);
    }

    public async Task<int> CountByStatusAsync(bool isActive)
    {
        return await _context.Users.CountAsync(u => u.IsActive == isActive);
    }
}