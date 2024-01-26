using Microsoft.Extensions.DependencyInjection;

namespace Map.Provider.Extensions;

public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Adds the providers.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>An IServiceCollection.</returns>
    public static IServiceCollection AddProviders(this IServiceCollection services)
    {

        return services;
    }
}