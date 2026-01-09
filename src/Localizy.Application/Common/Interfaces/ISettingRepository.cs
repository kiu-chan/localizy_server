using Localizy.Domain.Entities;

namespace Localizy.Application.Common.Interfaces;

public interface ISettingRepository
{
    Task<Setting?> GetByKeyAsync(string key);
    Task<IEnumerable<Setting>> GetAllAsync();
    Task<IEnumerable<Setting>> GetByCategoryAsync(string category);
    Task<Setting> CreateAsync(Setting setting);
    Task<Setting> UpdateAsync(Setting setting);
    Task DeleteAsync(string key);
    Task<bool> ExistsByKeyAsync(string key);
}