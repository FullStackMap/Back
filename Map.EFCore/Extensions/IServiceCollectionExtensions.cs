using Map.EFCore.Interfaces;
using Map.EFCore.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Map.EFCore.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }
}