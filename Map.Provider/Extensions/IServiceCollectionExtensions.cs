using Map.Provider.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Map.Provider.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddScoped<IMailProvider, MailProvider>();
        return services;
    }
}