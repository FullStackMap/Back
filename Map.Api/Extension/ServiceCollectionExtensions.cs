using Asp.Versioning;
using FluentValidation;
using Map.API.AutoMapperProfies;
using Map.API.Configuration;
using Map.API.Validator.AuthValidator;
using Map.API.Validator.StepValidator;
using Map.API.Validator.TestimonialValidator;
using Map.API.Validator.TravelValidator;
using Map.API.Validator.TripValidator;
using Map.API.Validator.UserValidator;
using Map.Domain.Entities;
using Map.Domain.Models.AddTravel;
using Map.Domain.Models.Auth;
using Map.Domain.Models.Step;
using Map.Domain.Models.Testimonial;
using Map.Domain.Models.Trip;
using Map.Domain.Models.User;
using Map.Domain.Settings;
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
    public static void AddAutoMapperConfiguration(this IServiceCollection services) => services
        .AddAutoMapper(typeof(TripProfiles))
        .AddAutoMapper(typeof(UserProfiles))
        .AddAutoMapper(typeof(StepProfiles))
        .AddAutoMapper(typeof(TravelProfiles))
        .AddAutoMapper(typeof(TestimonialProfiles));

    /// <summary>
    /// Adds the services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    public static void AddServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddSettings(configuration)
            .AddPlatforms()
            .AddProviders()
            .AddValidators()
            .AddRepositories()
            .AddDBInitializer()
            .AddIdentity();

        services.AddControllers();
    }

    /// <summary>
    /// Add settings as services
    /// </summary>
    /// <param name="services">The Services</param>
    public static IServiceCollection AddSettings(this IServiceCollection services, ConfigurationManager configuration)
    {
        JWTSettings JWTSettings = configuration.GetSection("JWTSettings").Get<JWTSettings>() ?? throw new ArgumentNullException(nameof(JWTSettings));
        services.AddSingleton(JWTSettings);

        MailSettings mailSettings = configuration.GetSection("MailSettings").Get<MailSettings>() ?? throw new ArgumentNullException(nameof(mailSettings));
        services.AddSingleton(mailSettings);

        RegisterSettings registerSettings = configuration.GetSection("RegisterSettings").Get<RegisterSettings>() ?? throw new ArgumentNullException(nameof(registerSettings));
        services.AddSingleton(registerSettings);

        return services;
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
        services.AddScoped<IValidator<UpdateTripDto>, UpdateTripValidator>();

        #endregion

        #region AuthValidator
        services.AddScoped<IValidator<LoginDto>, LoginValidator>();
        services.AddScoped<IValidator<RegisterDto>, RegisterValidator>();
        services.AddScoped<IValidator<ConfirmMailDto>, ConfirmMailValidator>();
        services.AddScoped<IValidator<ForgotPasswordDto>, ForgotPasswordValidator>();
        services.AddScoped<IValidator<ResetPasswordDto>, ResetPasswordValidator>();
        #endregion

        #region UserValidator
        services.AddScoped<IValidator<UpdateUserMailDto>, UpdateUserMailValidator>();
        #endregion

        #region StepValidator
        services.AddScoped<IValidator<AddStepDto>, AddStepValidator>();
        services.AddScoped<IValidator<UpdateStepDateDto>, UpdateStepDateValidator>();
        services.AddScoped<IValidator<UpdateStepDescriptionDto>, UpdateStepDescriptionValidator>();
        services.AddScoped<IValidator<UpdateStepLocationDto>, UpdateStepLocationValidator>();
        services.AddScoped<IValidator<UpdateStepNameDto>, UpdateStepNameValidator>();
        #endregion

        #region TravelValidator
        services.AddScoped<IValidator<AddTravelDto>, AddTravelValidator>();
        #endregion

        #region TestimonialValidator
        services.AddScoped<IValidator<AddTestimonialDto>, AddTestimonialValidator>();
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