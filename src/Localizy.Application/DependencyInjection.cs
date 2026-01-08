using Localizy.Application.Features.Users.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Localizy.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register Services
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}