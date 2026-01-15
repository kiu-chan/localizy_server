using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Localizy.Infrastructure.Services;

public interface IFileService
{
    Task<(string fileName, string filePath)> SaveFileAsync(IFormFile file, string folderName);
    Task DeleteFileAsync(string filePath);
    string GetFileUrl(string fileName, string folderName);
    string GetPhysicalPath(string fileName, string folderName);
}

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _environment;
    private readonly string _uploadPath;
    private readonly bool _isAzure;

    public FileService(IWebHostEnvironment environment)
    {
        _environment = environment;
        
        // Check if running on Azure
        _isAzure = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"));
        
        if (_isAzure)
        {
            // Azure: Use persistent storage
            _uploadPath = Path.Combine("/home", "uploads");
        }
        else
        {
            // Local: Use wwwroot
            var webRootPath = _environment.WebRootPath;
            if (string.IsNullOrEmpty(webRootPath))
            {
                webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
            }
            _uploadPath = Path.Combine(webRootPath, "uploads");
        }
        
        // Create uploads directory if not exists
        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }
    }

    public async Task<(string fileName, string filePath)> SaveFileAsync(IFormFile file, string folderName)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File is empty");
        }

        // Validate file extension
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        
        if (!allowedExtensions.Contains(fileExtension))
        {
            throw new ArgumentException("Invalid file type. Only images are allowed.");
        }

        // Validate file size (max 5MB)
        if (file.Length > 5 * 1024 * 1024)
        {
            throw new ArgumentException("File size exceeds 5MB limit.");
        }

        // Create folder path
        var folderPath = Path.Combine(_uploadPath, folderName);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Generate unique filename
        var fileName = $"{Guid.NewGuid()}{fileExtension}";
        var fullPath = Path.Combine(folderPath, fileName);

        // Save file
        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var fileUrl = GetFileUrl(fileName, folderName);
        return (fileName, fileUrl);
    }

    public async Task DeleteFileAsync(string filePath)
    {
        await Task.Run(() =>
        {
            // Convert URL path to physical path
            var physicalPath = GetPhysicalPathFromUrl(filePath);
            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }
        });
    }

    public string GetFileUrl(string fileName, string folderName)
    {
        return $"/uploads/{folderName}/{fileName}";
    }

    public string GetPhysicalPath(string fileName, string folderName)
    {
        return Path.Combine(_uploadPath, folderName, fileName);
    }

    private string GetPhysicalPathFromUrl(string urlPath)
    {
        // Convert "/uploads/home-slides/abc.jpg" to physical path
        var relativePath = urlPath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
        
        if (_isAzure)
        {
            return Path.Combine("/home", relativePath);
        }
        else
        {
            var webRootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
            return Path.Combine(webRootPath, relativePath);
        }
    }
}