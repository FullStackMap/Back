using Map.EFCore.Interfaces;
using Map.EFCore.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Map.EFCore.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped(typeof(ITripRepository), typeof(TripRepository));
        services.AddScoped(typeof(IStepRepository), typeof(StepRepository));
        services.AddScoped(typeof(ITravelRepository), typeof(TravelRepository));

        return services;
    }

    public static IServiceCollection AddDBInitializer(this IServiceCollection services)
    {
        services.AddScoped<DBInitializer>();

        return services;
    }
}