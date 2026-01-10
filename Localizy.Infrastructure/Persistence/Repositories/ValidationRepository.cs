using Localizy.Application.Common.Interfaces;
using Localizy.Domain.Entities;
using Localizy.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Localizy.Infrastructure.Persistence.Repositories;

public class ValidationRepository : IValidationRepository
{
    private readonly ApplicationDbContext _context;

    public ValidationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Validation?> GetByIdAsync(Guid id)
    {
        return await _context.Validations
            .Include(v => v.Address)
            .Include(v => v.SubmittedByUser)
            .Include(v => v.ProcessedByUser)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<Validation?> GetByRequestIdAsync(string requestId)
    {
        return await _context.Validations
            .Include(v => v.Address)
            .Include(v => v.SubmittedByUser)
            .Include(v => v.ProcessedByUser)
            .FirstOrDefaultAsync(v => v.RequestId == requestId);
    }

    public async Task<IEnumerable<Validation>> GetAllAsync()
    {
        return await _context.Validations
            .Include(v => v.Address)
            .Include(v => v.SubmittedByUser)
            .Include(v => v.ProcessedByUser)
            .OrderByDescending(v => v.SubmittedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Validation>> GetByStatusAsync(ValidationStatus status)
    {
        return await _context.Validations
            .Include(v => v.Address)
            .Include(v => v.SubmittedByUser)
            .Include(v => v.ProcessedByUser)
            .Where(v => v.Status == status)
            .OrderByDescending(v => v.SubmittedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Validation>> GetByPriorityAsync(ValidationPriority priority)
    {
        return await _context.Validations
            .Include(v => v.Address)
            .Include(v => v.SubmittedByUser)
            .Include(v => v.ProcessedByUser)
            .Where(v => v.Priority == priority)
            .OrderByDescending(v => v.SubmittedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Validation>> GetByUserAsync(Guid userId)
    {
        return await _context.Validations
            .Include(v => v.Address)
            .Include(v => v.SubmittedByUser)
            .Include(v => v.ProcessedByUser)
            .Where(v => v.SubmittedByUserId == userId)
            .OrderByDescending(v => v.SubmittedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Validation>> SearchAsync(string searchTerm)
    {
        var lowerSearchTerm = searchTerm.ToLower();
        
        return await _context.Validations
            .Include(v => v.Address)
            .Include(v => v.SubmittedByUser)
            .Include(v => v.ProcessedByUser)
            .Where(v => 
                v.RequestId.ToLower().Contains(lowerSearchTerm) ||
                v.Address.Name.ToLower().Contains(lowerSearchTerm) ||
                v.Address.FullAddress.ToLower().Contains(lowerSearchTerm) ||
                v.SubmittedByUser.FullName.ToLower().Contains(lowerSearchTerm) ||
                (v.Notes != null && v.Notes.ToLower().Contains(lowerSearchTerm))
            )
            .OrderByDescending(v => v.SubmittedDate)
            .ToListAsync();
    }

    public async Task<Validation> CreateAsync(Validation validation)
    {
        _context.Validations.Add(validation);
        await _context.SaveChangesAsync();
        
        // Load navigation properties
        await _context.Entry(validation)
            .Reference(v => v.Address)
            .LoadAsync();
        await _context.Entry(validation)
            .Reference(v => v.SubmittedByUser)
            .LoadAsync();
            
        return validation;
    }

    public async Task<Validation> UpdateAsync(Validation validation)
    {
        _context.Validations.Update(validation);
        await _context.SaveChangesAsync();
        
        // Reload navigation properties
        await _context.Entry(validation)
            .Reference(v => v.Address)
            .LoadAsync();
        await _context.Entry(validation)
            .Reference(v => v.SubmittedByUser)
            .LoadAsync();
        await _context.Entry(validation)
            .Reference(v => v.ProcessedByUser)
            .LoadAsync();
            
        return validation;
    }

    public async Task DeleteAsync(Guid id)
    {
        var validation = await _context.Validations.FindAsync(id);
        if (validation != null)
        {
            _context.Validations.Remove(validation);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Validations.AnyAsync(v => v.Id == id);
    }

    public async Task<int> CountByStatusAsync(ValidationStatus status)
    {
        return await _context.Validations.CountAsync(v => v.Status == status);
    }

    public async Task<int> CountByPriorityAsync(ValidationPriority priority)
    {
        return await _context.Validations.CountAsync(v => v.Priority == priority);
    }

    public async Task<int> CountTodayAsync()
    {
        var today = DateTime.UtcNow.Date;
        return await _context.Validations
            .CountAsync(v => v.SubmittedDate.Date == today);
    }

    public async Task<string> GenerateRequestIdAsync()
    {
        var year = DateTime.UtcNow.Year;
        var lastValidation = await _context.Validations
            .Where(v => v.RequestId.StartsWith($"VAL-{year}-"))
            .OrderByDescending(v => v.CreatedAt)
            .FirstOrDefaultAsync();

        int nextNumber = 1;
        if (lastValidation != null)
        {
            var parts = lastValidation.RequestId.Split('-');
            if (parts.Length == 3 && int.TryParse(parts[2], out int lastNumber))
            {
                nextNumber = lastNumber + 1;
            }
        }

        return $"VAL-{year}-{nextNumber:D3}";
    }
}