using FluentValidation;
using Map.API.AutoMapperProfies;
using Map.API.Models.TripDto;
using Map.API.Validator.TripValidator;
using Map.Domain.Entities;
using Map.EFCore;
using Map.EFCore.Extensions;
using Map.Platform.Extensions;
using Map.Provider.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Map.API.Extension;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures the swagger.
    /// </summary>
    /// <param name="services">The services.</param>
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            string? API_NAME = Assembly.GetExecutingAssembly().GetName().Name;
            string xmlPath = $"{AppContext.BaseDirectory}{API_NAME}.xml";

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = API_NAME,
                Description = "MAP API",
            });
            c.IncludeXmlComments(xmlPath);
        });
    }

    /// <summary>
    /// Configures the cors.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    public static void ConfigureCors(this IServiceCollection services, ConfigurationManager configuration)
    {
        List<string> originsAllowed = configuration.GetSection("CallsOrigins").Get<List<string>>()!;
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                       .WithOrigins(originsAllowed.ToArray())
                       .WithMethods("PUT", "DELETE", "GET", "OPTIONS", "POST")
                       .AllowAnyHeader()
                       .Build();
            });
        });
    }

    /// <summary>
    /// Adds the auto mapper configuration.
    /// </summary>
    /// <param name="services">The services.</param>
    public static void AddAutoMapperConfiguration(this IServiceCollection services) => services.AddAutoMapper(typeof(TripProfiles));


    /// <summary>
    /// Adds the services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    public static void AddServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddPlatforms()
            .AddProviders()
            .AddValidators()
            .AddRepositories()
            .AddIdentity();

        services.AddControllers();
    }

    /// <summary>
    /// Adds the validators.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>An IServiceCollection.</returns>
    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        #region TripValidator

        services.AddScoped<IValidator<AddTripDto>, AddTripValidator>();

        #endregion
        return services;
    }

}