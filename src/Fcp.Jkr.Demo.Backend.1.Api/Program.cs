using System.Diagnostics.CodeAnalysis;

using Fcp.Jkr.Demo.Backend.1.Api.AutoMapper;
using Fcp.Jkr.Demo.Backend.1.Api.Config;
using Fcp.Jkr.Demo.Backend.1.Api.Extensions;
using Fcp.Jkr.Demo.Backend.1.Api.HealthChecks;

using Asp.Versioning;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

namespace Fcp.Jkr.Demo.Backend.1.Api;

[ExcludeFromCodeCoverage]
public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        ConfigureServices(builder);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }
        app.UseExceptionHandler();

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.UseHealthChecks("/healthy", new HealthCheckOptions()
        {
            Predicate = check => check.Name == "ready"
        });
        app.UseHealthChecks("/healthz", new HealthCheckOptions()
        {
            Predicate = check => check.Name == "live"
        });

        app.Run();
    }

    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddApplicationServices();
        builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

        builder.Services.AddLogging();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddHealthChecks()
               .AddCheck<LivenessCheck>("live")
               .AddCheck<ReadinessCheck>("ready");

        var appInsightsConfig = new AppInsightsConfig
        {
            CloudRole = builder.Configuration.GetValue<string>("APPINSIGHTS_CLOUDROLE") ?? "",
            ConnectionString = builder.Configuration.GetValue<string>("APPINSIGHTS_CONNECTIONSTRING") ?? ""
        };

        builder.ConfigureOpenTelemetry(appInsightsConfig);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v1", new OpenApiInfo { Title = "Fcp.Jkr.Demo.Backend.1", Version = "v1" });
        });
        builder.Services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
            config.ApiVersionReader = new HeaderApiVersionReader("api-version");
        });
    }
}
