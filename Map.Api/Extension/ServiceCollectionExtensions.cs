using Asp.Versioning;
using FluentValidation;
using Map.API.AutoMapperProfies;
using Map.API.Configuration;
using Map.API.Models.TripDto;
using Map.API.Validator.TripValidator;
using Map.Domain.Entities;
using Map.EFCore;
using Map.EFCore.Extensions;
using Map.Platform.Extensions;
using Map.Provider.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Security.Claims;

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
        services.AddSingleton<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();
        services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());
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
            .AddDBInitializer()
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

    private static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<MapUser, IdentityRole<Guid>>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;

            options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
            options.ClaimsIdentity.UserIdClaimType = "Id";
            options.ClaimsIdentity.UserNameClaimType = "UserName";
            options.ClaimsIdentity.EmailClaimType = ClaimTypes.Email;

            //Password requirement
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 4; //Determine le nombre de caractère unnique minimum requis

            //Lockout si mdp fail 5 fois alors compte bloquer 60 min
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
            options.Lockout.AllowedForNewUsers = true;

            //User
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;

            //Sign
            options.SignIn.RequireConfirmedAccount = true;
        })
        .AddDefaultTokenProviders()
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<MapContext>();

        services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };
        });
        return services;
    }

    public static void ConfigureApiVersionning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = $"'v'VVV";
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.SubstituteApiVersionInUrl = true;
            options.SubstitutionFormat = "VVV";
        });
    }
}