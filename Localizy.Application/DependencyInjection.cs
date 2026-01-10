using Localizy.Application.Features.Addresses.Services;
using Localizy.Application.Features.Auth.Services;
using Localizy.Application.Features.Settings.Services;
using Localizy.Application.Features.Users.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Localizy.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ISettingService, SettingService>();
        services.AddScoped<IAddressService, AddressService>();

        return services;
    }
}