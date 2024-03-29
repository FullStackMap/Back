﻿using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Map.API.Configuration;

public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

    public ConfigureSwaggerGenOptions(
        IApiVersionDescriptionProvider apiVersionDescriptionProvider) => _apiVersionDescriptionProvider = apiVersionDescriptionProvider ?? throw new ArgumentNullException(nameof(apiVersionDescriptionProvider));

    /// <summary>
    ///
    /// </summary>
    /// <param name="options"></param>
    public void Configure(SwaggerGenOptions options)
    {
        string? API_NAME = Assembly.GetExecutingAssembly().GetName().Name;
        foreach (ApiVersionDescription description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
            options.SwaggerDoc(description.GroupName,
                new OpenApiInfo
                {
                    Title = API_NAME,
                    Version = description.GroupName,
                });

        string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

        options.CustomOperationIds(apiDescription => apiDescription.TryGetMethodInfo(out MethodInfo methodInfo) ? $"{methodInfo.Name}{apiDescription.HttpMethod}" : null);
        options.AddSecurityDefinition("Bearer", new()
        {
            Description = """
            JWT authorization header using the bearer scheme. <br />
            Enter your token in the text input below. <br />
            Example: '12345abcdef' <br />
            NB: A cookie will be added in order to remember the authentication during the session,
            if you try to logout using swagger logout feature this might not work.
            """,
            Name = "Authorization",
            In = ParameterLocation.Header,
            Scheme = "Bearer",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
        });

        options.AddSecurityRequirement(new()
        {
            {
                new()
                {
                    Reference = new()
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                },
                new string[]{}
            }
        });
    }
}
