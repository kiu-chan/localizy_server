using Localizy.Application.Features.Settings.DTOs;

namespace Localizy.Application.Features.Settings.Services;

public interface ISettingService
{
    Task<WebsiteConfigDto> GetWebsiteConfigAsync();
    Task<IEnumerable<SettingDto>> GetAllAsync();
    Task<IEnumerable<SettingDto>> GetByCategoryAsync(string category);
    Task<SettingDto?> GetByKeyAsync(string key);
    Task<SettingDto> UpdateAsync(string key, UpdateSettingDto dto);
}