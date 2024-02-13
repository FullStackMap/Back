using Map.Platform.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Map.Platform.Extensions;

public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Adds the platforms.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>An IServiceCollection.</returns>
    public static IServiceCollection AddPlatforms(this IServiceCollection services)
    {
        services.AddScoped<IAuthPlatform, AuthPlatform>();
        services.AddScoped<ITripPlatform, TripPlatform>();
        services.AddScoped<IMailPlatform, MailPlatform>();

        return services;
    }
}