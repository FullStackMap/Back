using Microsoft.Extensions.DependencyInjection;

namespace Map.Provider.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddProviders(this IServiceCollection services)
    {
        return services;
    }
}