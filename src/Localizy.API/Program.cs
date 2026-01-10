using DotNetEnv;
using Localizy.Application;
using Localizy.Infrastructure;
using Localizy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables
Env.Load();

// Build connection string from environment variables
var server = Environment.GetEnvironmentVariable("DB_SERVER");
var database = Environment.GetEnvironmentVariable("DB_DATABASE");
var userId = Environment.GetEnvironmentVariable("DB_USER_ID");
var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
var connectionString = $"Server={server};Database={database};User Id={userId};Password={password};TrustServerCertificate=True;MultipleActiveResultSets=true";

// Override configuration
builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;

// JWT Settings from environment
builder.Configuration["JwtSettings:Secret"] = Environment. GetEnvironmentVariable("JWT_SECRET") ?? "your-super-secret-key-at-least-32-characters-long-for-security";
builder.Configuration["JwtSettings:Issuer"] = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "LocalizyAPI";
builder.Configuration["JwtSettings:Audience"] = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "LocalizyClient";
builder.Configuration["JwtSettings:ExpirationInMinutes"] = Environment.GetEnvironmentVariable("JWT_EXPIRATION_MINUTES") ?? "1440";

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",  // Vite default port
                "http://localhost:3000",  // React default port
                "http://localhost:4200",  // Angular default port
                "http://localhost:8080"   // Vue default port
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Add services to the container.
builder. Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Application & Infrastructure
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
        await DataSeeder.SeedAsync(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding data: {ex. Message}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// IMPORTANT: UseCors phải đứng trước UseAuthentication và UseAuthorization
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();