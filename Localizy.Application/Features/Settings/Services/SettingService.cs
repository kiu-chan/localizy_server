using Localizy.Application.Common.Interfaces;
using Localizy.Application.Features.Settings.DTOs;
using Localizy.Domain.Enums;

namespace Localizy.Application.Features.Settings.Services;

public class SettingService : ISettingService
{
    private readonly ISettingRepository _settingRepository;

    public SettingService(ISettingRepository settingRepository)
    {
        _settingRepository = settingRepository;
    }

    public async Task<WebsiteConfigDto> GetWebsiteConfigAsync()
    {
        var settings = await _settingRepository.GetAllAsync();
        var settingsDict = settings.ToDictionary(s => s.Key, s => s.Value);

        return new WebsiteConfigDto
        {
            AppDownload = new AppDownloadLinks
            {
                IosLink = GetValue(settingsDict, SettingKey.IosAppLink),
                AndroidLink = GetValue(settingsDict, SettingKey.AndroidAppLink)
            },
            SocialMedia = new SocialMediaLinks
            {
                Facebook = GetValue(settingsDict, SettingKey.FacebookLink),
                Twitter = GetValue(settingsDict, SettingKey.TwitterLink),
                Instagram = GetValue(settingsDict, SettingKey.InstagramLink),
                LinkedIn = GetValue(settingsDict, SettingKey.LinkedInLink),
                Youtube = GetValue(settingsDict, SettingKey.YoutubeLink)
            },
            Contact = new ContactInfo
            {
                Email = GetValue(settingsDict, SettingKey.Email),
                Phone = GetValue(settingsDict, SettingKey.Phone),
                Address = GetValue(settingsDict, SettingKey.Address)
            },
            General = new GeneralInfo
            {
                Slogan = GetValue(settingsDict, SettingKey.Slogan),
                Description = GetValue(settingsDict, SettingKey.Description),
                AboutUs = GetValue(settingsDict, SettingKey.AboutUs)
            }
        };
    }

    public async Task<IEnumerable<SettingDto>> GetAllAsync()
    {
        var settings = await _settingRepository.GetAllAsync();
        return settings.Select(MapToDto);
    }

    public async Task<IEnumerable<SettingDto>> GetByCategoryAsync(string category)
    {
        var settings = await _settingRepository.GetByCategoryAsync(category);
        return settings.Select(MapToDto);
    }

    public async Task<SettingDto?> GetByKeyAsync(string key)
    {
        var setting = await _settingRepository.GetByKeyAsync(key);
        return setting == null ? null : MapToDto(setting);
    }

    public async Task<SettingDto> UpdateAsync(string key, UpdateSettingDto dto)
    {
        var setting = await _settingRepository.GetByKeyAsync(key);
        if (setting == null)
        {
            throw new KeyNotFoundException($"Setting với key '{key}' không tồn tại");
        }

        setting.Value = dto.Value;
        if (dto.Description != null)
        {
            setting.Description = dto.Description;
        }

        var updatedSetting = await _settingRepository.UpdateAsync(setting);
        return MapToDto(updatedSetting);
    }

    private static string? GetValue(Dictionary<string, string> dict, string key)
    {
        return dict.TryGetValue(key, out var value) ? value : null;
    }

    private static SettingDto MapToDto(Domain.Entities.Setting setting)
    {
        return new SettingDto
        {
            Id = setting.Id,
            Key = setting.Key,
            Value = setting.Value,
            Description = setting.Description,
            Category = setting.Category
        };
    }
}