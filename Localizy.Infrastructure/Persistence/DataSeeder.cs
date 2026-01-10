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
            var address1 = new Address
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
                Description = "Hồ nước ngọt nằm ở trung tâm Hà Nội",
                Status = AddressStatus.Verified,
                Rating = 4.8,
                Views = 15234,
                SubmittedByUserId = regularUser.Id,
                SubmittedDate = DateTime.UtcNow.AddDays(-30),
                VerifiedByUserId = admin.Id,
                VerifiedDate = DateTime.UtcNow.AddDays(-29),
                CreatedAt = DateTime.UtcNow.AddDays(-30)
            };

            var address2 = new Address
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
                Description = "Quán phở nổi tiếng",
                Phone = "+84 24 3942 8866",
                Status = AddressStatus.Pending,
                Rating = 4.5,
                Views = 3421,
                SubmittedByUserId = regularUser.Id,
                SubmittedDate = DateTime.UtcNow.AddDays(-2),
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            };

            var address3 = new Address
            {
                Id = Guid.NewGuid(),
                Name = "Bảo Tàng Lịch Sử",
                FullAddress = "1 Tràng Tiền, Hoàn Kiếm, Hà Nội",
                City = "Hà Nội",
                Country = "Việt Nam",
                Type = "Museum",
                Category = "History Museum",
                Latitude = 21.0245,
                Longitude = 105.8571,
                Description = "Bảo tàng lịch sử Việt Nam",
                Status = AddressStatus.Pending,
                Rating = 4.4,
                Views = 5678,
                SubmittedByUserId = regularUser.Id,
                SubmittedDate = DateTime.UtcNow.AddDays(-5),
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            };

            context.Addresses.AddRange(address1, address2, address3);
            await context.SaveChangesAsync();

            // Seed sample validations
            var validation1 = new Validation
            {
                Id = Guid.NewGuid(),
                RequestId = "VAL-2024-001",
                AddressId = address2.Id,
                Status = ValidationStatus.Pending,
                Priority = ValidationPriority.High,
                RequestType = ValidationRequestType.NewAddress,
                SubmittedByUserId = regularUser.Id,
                SubmittedDate = DateTime.UtcNow.AddDays(-2),
                Notes = "Quán phở nổi tiếng cần xác thực thông tin",
                PhotosProvided = true,
                DocumentsProvided = true,
                LocationVerified = false,
                AttachmentsCount = 3,
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            };

            var validation2 = new Validation
            {
                Id = Guid.NewGuid(),
                RequestId = "VAL-2024-002",
                AddressId = address1.Id,
                Status = ValidationStatus.Verified,
                Priority = ValidationPriority.Medium,
                RequestType = ValidationRequestType.UpdateInformation,
                SubmittedByUserId = regularUser.Id,
                SubmittedDate = DateTime.UtcNow.AddDays(-30),
                Notes = "Cập nhật thông tin giờ mở cửa",
                OldData = "{\"openingHours\":\"24/7\"}",
                NewData = "{\"openingHours\":\"06:00 - 22:00\"}",
                PhotosProvided = true,
                DocumentsProvided = false,
                LocationVerified = true,
                AttachmentsCount = 1,
                ProcessedByUserId = admin.Id,
                ProcessedDate = DateTime.UtcNow.AddDays(-29),
                ProcessingNotes = "Đã xác thực thông tin từ nguồn chính thức",
                CreatedAt = DateTime.UtcNow.AddDays(-30)
            };

            var validation3 = new Validation
            {
                Id = Guid.NewGuid(),
                RequestId = "VAL-2024-003",
                AddressId = address3.Id,
                Status = ValidationStatus.Pending,
                Priority = ValidationPriority.Medium,
                RequestType = ValidationRequestType.NewAddress,
                SubmittedByUserId = regularUser.Id,
                SubmittedDate = DateTime.UtcNow.AddDays(-5),
                Notes = "Bảo tàng mới cần xác thực",
                PhotosProvided = true,
                DocumentsProvided = true,
                LocationVerified = false,
                AttachmentsCount = 5,
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            };

            context.Validations.AddRange(validation1, validation2, validation3);
            await context.SaveChangesAsync();
            
            Console.WriteLine("✓ Sample addresses and validations created");
        }

        // Seed Settings
        if (!await context.Settings.AnyAsync())
        {
            var settings = new List<Setting>
            {
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