using Asp.Versioning.ApiExplorer;
using Map.API.Extension;
using Map.EFCore.IOC;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetupConfiguration();

builder.Services.ConfigureCache();

// Add Controllers to services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Ajout du MapContext
builder.Services.AddMapDbContext(builder.Configuration);

//Configure Cors
builder.Services.ConfigureCors(builder.Configuration);

//Configre API Versioning
builder.Services.ConfigureApiVersionning();

//Add services to container
builder.Services.AddServices(builder.Configuration);

//Configure swagger
builder.Services.ConfigureSwagger();

//Add AutoMapper
builder.Services.AddAutoMapperConfiguration();

WebApplication app = builder.Build();

app.Services.ConfigureDatabase();

bool displaySwagger = builder.Configuration.GetValue<bool>("DisplaySwagger");
if (displaySwagger)
{
    IApiVersionDescriptionProvider apiVersionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        options.DisplayOperationId();
        options.DisplayRequestDuration();
        options.EnableFilter();
        options.EnableTryItOutByDefault();

        foreach (ApiVersionDescription description in apiVersionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseOutputCache();

app.MapControllers();

app.Run();

public partial class Program
{
    protected Program()
    {
    }
}