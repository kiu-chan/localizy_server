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
                Location = "Hanoi, Vietnam",
                Phone = "+84 123 456 789",
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(admin);
            await context.SaveChangesAsync();
            
            Console.WriteLine("✓ Admin user created: admin@localizy.com / Admin@123");

            // Seed sample regular user
            var regularUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "user@localizy.com",
                FullName = "John Smith",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
                IsActive = true,
                Role = UserRole.User,
                Location = "Ho Chi Minh, Vietnam",
                Phone = "+84 987 654 321",
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(regularUser);
            await context.SaveChangesAsync();

            Console.WriteLine("✓ Regular user created: user@localizy.com / User@123");

            // Seed sample addresses
            var sampleAddresses = new List<Address>
            {
                new Address
                {
                    Id = Guid.NewGuid(),
                    Name = "Hồ Hoàn Kiếm",
                    FullAddress = "Đinh Tiên Hoàng, Hoàn Kiếm, Hà Nội",
                    City = "Hà Nội",
                    Country = "Việt Nam",
                    Type = "Landmark",
                    Category = "Lake",
                    Latitude = 21.0285,
                    Longitude = 105.8542,
                    Description = "Hồ nước ngọt nằm ở trung tâm Hà Nội, biểu tượng của thủ đô",
                    Status = AddressStatus.Verified,
                    Rating = 4.8,
                    Views = 15234,
                    SubmittedByUserId = regularUser.Id,
                    SubmittedDate = DateTime.UtcNow.AddDays(-30),
                    VerifiedByUserId = admin.Id,
                    VerifiedDate = DateTime.UtcNow.AddDays(-29),
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                },
                new Address
                {
                    Id = Guid.NewGuid(),
                    Name = "Nhà Thờ Lớn Hà Nội",
                    FullAddress = "40 Nhà Chung, Hoàn Kiếm, Hà Nội",
                    City = "Hà Nội",
                    Country = "Việt Nam",
                    Type = "Religious",
                    Category = "Cathedral",
                    Latitude = 21.0288,
                    Longitude = 105.8489,
                    Description = "Nhà thờ Công giáo Gothic được xây dựng vào năm 1886",
                    Phone = "+84 24 3828 5967",
                    Status = AddressStatus.Verified,
                    Rating = 4.6,
                    Views = 8765,
                    SubmittedByUserId = regularUser.Id,
                    SubmittedDate = DateTime.UtcNow.AddDays(-25),
                    VerifiedByUserId = admin.Id,
                    VerifiedDate = DateTime.UtcNow.AddDays(-24),
                    CreatedAt = DateTime.UtcNow.AddDays(-25)
                },
                new Address
                {
                    Id = Guid.NewGuid(),
                    Name = "Phở Thìn Bờ Hồ",
                    FullAddress = "13 Lò Đúc, Hai Bà Trưng, Hà Nội",
                    City = "Hà Nội",
                    Country = "Việt Nam",
                    Type = "Restaurant",
                    Category = "Vietnamese Restaurant",
                    Latitude = 21.0245,
                    Longitude = 105.8516,
                    Description = "Quán phở nổi tiếng với món phở bò trộn đặc trưng",
                    Phone = "+84 24 3942 8866",
                    OpeningHours = "06:00 - 22:00",
                    Status = AddressStatus.Pending,
                    Rating = 4.5,
                    Views = 3421,
                    SubmittedByUserId = regularUser.Id,
                    SubmittedDate = DateTime.UtcNow.AddDays(-2),
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new Address
                {
                    Id = Guid.NewGuid(),
                    Name = "Bảo Tàng Lịch Sử Quốc Gia",
                    FullAddress = "1 Tràng Tiền, Hoàn Kiếm, Hà Nội",
                    City = "Hà Nội",
                    Country = "Việt Nam",
                    Type = "Museum",
                    Category = "History Museum",
                    Latitude = 21.0245,
                    Longitude = 105.8571,
                    Description = "Bảo tàng trưng bày hiện vật lịch sử Việt Nam",
                    Phone = "+84 24 3825 2853",
                    Website = "www.baotanglichsu.vn",
                    OpeningHours = "08:00 - 17:00 (Đóng cửa thứ Hai)",
                    Status = AddressStatus.Verified,
                    Rating = 4.4,
                    Views = 5678,
                    SubmittedByUserId = regularUser.Id,
                    SubmittedDate = DateTime.UtcNow.AddDays(-20),
                    VerifiedByUserId = admin.Id,
                    VerifiedDate = DateTime.UtcNow.AddDays(-19),
                    CreatedAt = DateTime.UtcNow.AddDays(-20)
                },
                new Address
                {
                    Id = Guid.NewGuid(),
                    Name = "Chợ Đồng Xuân",
                    FullAddress = "Đồng Xuân, Hoàn Kiếm, Hà Nội",
                    City = "Hà Nội",
                    Country = "Việt Nam",
                    Type = "Shopping",
                    Category = "Market",
                    Latitude = 21.0364,
                    Longitude = 105.8477,
                    Description = "Chợ truyền thống lớn nhất Hà Nội",
                    OpeningHours = "06:00 - 19:00",
                    Status = AddressStatus.Pending,
                    Rating = 4.2,
                    Views = 2341,
                    SubmittedByUserId = regularUser.Id,
                    SubmittedDate = DateTime.UtcNow.AddDays(-1),
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                }
            };

            context.Addresses.AddRange(sampleAddresses);
            await context.SaveChangesAsync();
            
            Console.WriteLine("✓ Sample addresses created");
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
                    Description = "Link tải ứng dụng iOS",
                    Category = SettingCategory.AppDownload,
                    CreatedAt = DateTime.UtcNow
                },
                new Setting
                {
                    Id = Guid.NewGuid(),
                    Key = SettingKey.AndroidAppLink,
                    Value = "https://play.google.com/store/apps/details?id=com.localizy",
                    Description = "Link tải ứng dụng Android",
                    Category = SettingCategory.AppDownload,
                    CreatedAt = DateTime.UtcNow
                },

                // Social Media Links
                new Setting
                {
                    Id = Guid.NewGuid(),
                    Key = SettingKey.FacebookLink,
                    Value = "https://facebook.com/localizy",
                    Description = "Link Facebook",
                    Category = SettingCategory.SocialMedia,
                    CreatedAt = DateTime.UtcNow
                },
                new Setting
                {
                    Id = Guid.NewGuid(),
                    Key = SettingKey.TwitterLink,
                    Value = "https://twitter.com/localizy",
                    Description = "Link Twitter",
                    Category = SettingCategory.SocialMedia,
                    CreatedAt = DateTime.UtcNow
                },
                new Setting
                {
                    Id = Guid.NewGuid(),
                    Key = SettingKey.InstagramLink,
                    Value = "https://instagram.com/localizy",
                    Description = "Link Instagram",
                    Category = SettingCategory.SocialMedia,
                    CreatedAt = DateTime.UtcNow
                },

                // Contact Information
                new Setting
                {
                    Id = Guid.NewGuid(),
                    Key = SettingKey.Email,
                    Value = "contact@localizy.com",
                    Description = "Email liên hệ",
                    Category = SettingCategory.Contact,
                    CreatedAt = DateTime.UtcNow
                },
                new Setting
                {
                    Id = Guid.NewGuid(),
                    Key = SettingKey.Phone,
                    Value = "+84 123 456 789",
                    Description = "Số điện thoại liên hệ",
                    Category = SettingCategory.Contact,
                    CreatedAt = DateTime.UtcNow
                },

                // General Information
                new Setting
                {
                    Id = Guid.NewGuid(),
                    Key = SettingKey.Slogan,
                    Value = "Localizy - Khám phá địa điểm dễ dàng",
                    Description = "Khẩu hiệu của website",
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