using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Map.EFCore.IOC;

public static class ServiceCollectionExtensions
{
    #region Methods

    /// <summary>
    /// Adds the MapContext.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    public static void AddMapDbContext(this IServiceCollection services, ConfigurationManager configuration)
    {
        bool? inPipeline = Environment.GetEnvironmentVariable("USE_IN_MEMORY_DATABASE")?.ToLower() == "true";
        string? connectionString = configuration.GetConnectionString("Map_SQL");

        if (inPipeline is true)
        {
            services.AddDbContext<MapContext>(options =>
                options.UseInMemoryDatabase("InMemoryDbMap"));
        }
        else
        {
            services.AddDbContext<MapContext>(options =>
                options.UseSqlServer(
                    string.IsNullOrWhiteSpace(connectionString) ? Environment.GetEnvironmentVariable("CONNECTION_STRING") : connectionString,
                    x => x.MigrationsAssembly(typeof(MapContext).Assembly.FullName)));
        }

    }

    /// <summary>
    /// Configures the database.
    /// </summary>
    /// <param name="services">The services.</param>
    public static void ConfigureDatabase(this IServiceProvider services)
    {
        using (IServiceScope serviceScope = services.CreateScope())
        {
            MapContext? context = serviceScope.ServiceProvider.GetService<MapContext>();
            if (context != null)
            {
                if (context.Database.IsRelational())
                {
                    context?.Database.Migrate();
                }
                else
                {
                    context.Database.EnsureCreated();
                }
            }
        }
    }

    #endregion Methods
}