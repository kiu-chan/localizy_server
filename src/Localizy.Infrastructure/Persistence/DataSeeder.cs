using Localizy.Domain.Entities;
using Localizy.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Localizy.Infrastructure.Persistence;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Seed Admin user
        if (!await context.Users.AnyAsync(u => u.Role == UserRole.Admin))
        {
            var admin = new User
            {
                Id = Guid.NewGuid(),
                Email = "admin@localizy.com",
                FullName = "System Administrator",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                IsActive = true,
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(admin);
            await context.SaveChangesAsync();
            
            Console.WriteLine("✓ Admin user created: admin@localizy.com / Admin@123");
        }

        // Seed Settings
        if (!await context.Settings.AnyAsync())
        {
            var settings = new List<Setting>
            {
                // App Download Links
                new Setting
                {
                    Id = Guid.NewGuid(),
                    Key = SettingKey.IosAppLink,
                    Value = "https://apps.apple.com/app/localizy",
                    Description = "iOS App download link",
                    Category = SettingCategory.AppDownload,
                    CreatedAt = DateTime.UtcNow
                },
                new Setting
                {
                    Id = Guid.NewGuid(),
                    Key = SettingKey.AndroidAppLink,
                    Value = "https://play.google.com/store/apps/details?id=com.localizy",
                    Description = "Android App download link",
                    Category = SettingCategory.AppDownload,
                    CreatedAt = DateTime.UtcNow
                },

                // Social Media Links
                new Setting
                {
                    Id = Guid.NewGuid(),
                    Key = SettingKey.FacebookLink,
                    Value = "https://facebook.com/localizy",
                    Description = "Facebook page link",
                    Category = SettingCategory.SocialMedia,
                    CreatedAt = DateTime.UtcNow
                },
                new Setting
                {
                    Id = Guid.NewGuid(),
                    Key = SettingKey.TwitterLink,
                    Value = "https://twitter.com/localizy",
                    Description = "Twitter profile link",
                    Category = SettingCategory.SocialMedia,
                    CreatedAt = DateTime.UtcNow
                },
                new Setting
                {
                    Id = Guid.NewGuid(),
                    Key = SettingKey.InstagramLink,
                    Value = "https://instagram.com/localizy",
                    Description = "Instagram profile link",
                    Category = SettingCategory.SocialMedia,
                    CreatedAt = DateTime.UtcNow
                },
                new Setting
                {
                    Id = Guid.NewGuid(),
                    Key = SettingKey.LinkedInLink,
                    Value = "https://linkedin.com/company/localizy",
                    Description = "LinkedIn company link",
                    Category = SettingCategory.SocialMedia,
                    CreatedAt = DateTime.UtcNow
                },
                new Setting
                {
                    Id = Guid.NewGuid(),
                    Key = SettingKey.YoutubeLink,
                    Value = "https://youtube.com/@localizy",
                    Description = "YouTube channel link",
                    Category = SettingCategory.SocialMedia,
                    CreatedAt = DateTime.UtcNow
                },

                // Contact Information
                new Setting
                {
                    Id = Guid.NewGuid(),
                    Key = SettingKey.Email,
                    Value = "contact@localizy.com",
                    Description = "Contact email address",
                    Category = SettingCategory.Contact,
                    CreatedAt = DateTime.UtcNow
                },
                new Setting
                {
                    Id = Guid.NewGuid(),
                    Key = SettingKey.Phone,
                    Value = "+84 123 456 789",
                    Description = "Contact phone number",
                    Category = SettingCategory.Contact,
                    CreatedAt = DateTime.UtcNow
                },
                new Setting
                {
                    Id = Guid.NewGuid(),
                    Key = SettingKey.Address,
                    Value = "123 ABC Street, District 1, Ho Chi Minh City",
                    Description = "Office address",
                    Category = SettingCategory.Contact,
                    CreatedAt = DateTime.UtcNow
                },

                // General Information
                new Setting
                {
                    Id = Guid.NewGuid(),
                    Key = SettingKey.Slogan,
                    Value = "Localizy - Multi-language management made easy",
                    Description = "Website slogan",
                    Category = SettingCategory.General,
                    CreatedAt = DateTime.UtcNow
                },
                new Setting
                {
                    Id = Guid.NewGuid(),
                    Key = SettingKey.Description,
                    Value = "Localizy is a multi-language management platform for your applications",
                    Description = "Short website description",
                    Category = SettingCategory.General,
                    CreatedAt = DateTime.UtcNow
                },
                new Setting
                {
                    Id = Guid.NewGuid(),
                    Key = SettingKey.AboutUs,
                    Value = "We provide comprehensive multi-language management solutions for modern applications. With Localizy, managing translations becomes simpler and more efficient than ever.",
                    Description = "Detailed company introduction",
                    Category = SettingCategory.General,
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.Settings.AddRange(settings);
            await context.SaveChangesAsync();
            
            Console.WriteLine("✓ Default settings created");
        }
    }
}