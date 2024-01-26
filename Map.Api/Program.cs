using Map.API.Extension;
using Map.EFCore.IOC;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add Controllers to services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMapDbContext(builder.Configuration);
//Configure swagger
builder.Services.ConfigureSwagger();

//Configure Cors
builder.Services.ConfigureCors(builder.Configuration);

//Add services to container
builder.Services.AddServices(builder.Configuration);
//Add AutoMapper
builder.Services.AddAutoMapperConfiguration();

WebApplication app = builder.Build();

app.Services.ConfigureDatabase();

bool displaySwagger = builder.Configuration.GetValue<bool>("DisplaySwagger");
if (displaySwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        options.DisplayOperationId();
        options.DisplayRequestDuration();
        options.EnableFilter();
        options.EnableTryItOutByDefault();
    });
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
    protected Program()
    {
    }
}