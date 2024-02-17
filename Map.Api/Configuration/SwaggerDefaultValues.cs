﻿using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace Map.API.Configuration;

public class SwaggerDefaultValues : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        ApiDescription apiDescription = context.ApiDescription;

        operation.Deprecated |= apiDescription.IsDeprecated();

        foreach (ApiResponseType responseType in context.ApiDescription.SupportedResponseTypes)
        {
            string responseKey = responseType.IsDefaultResponse
                              ? "default"
                              : responseType.StatusCode.ToString();
            OpenApiResponse response = operation.Responses[responseKey];

            foreach (string? contentType in response.Content.Keys)
            {
                if (!responseType.ApiResponseFormats.Any(x => x.MediaType == contentType))
                {
                    response.Content.Remove(contentType);
                }
            }
        }

        if (operation.Parameters == null)
        {
            return;
        }

        foreach (OpenApiParameter? parameter in operation.Parameters)
        {
            ApiParameterDescription description = apiDescription.ParameterDescriptions
                                            .First(p => p.Name == parameter.Name);

            parameter.Description ??= description.ModelMetadata?.Description;

            if (parameter.Schema.Default == null && description.DefaultValue != null && description.ModelMetadata != null)
            {
                string json = JsonSerializer.Serialize(
                    description.DefaultValue,
                    description.ModelMetadata.ModelType);
                parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
            }

            parameter.Required |= description.IsRequired;
        }
    }
}
