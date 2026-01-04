using GameStore.Data;
using GameStore.Endpoints;
using GameStore.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0); // Set the default API version to 1.0 if a version is not specified in the request.
    config.AssumeDefaultVersionWhenUnspecified = true; // Assume default version when not specified
    config.ReportApiVersions = true; // To Let clients know which API versions are supported
    config.ApiVersionReader = new HeaderApiVersionReader("api-version"); // To pass the version information in the request header.
});

// Configure Serilog
//builder.Host.UseSerilog((context, config) =>
//{
//    config.MinimumLevel.Information()
//        .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
//        .WriteTo.Debug(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {level:u3} {Message:lj}{NewLine}{Exception}")
//        .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {level:u3} {Message:lj}{NewLine}{Exception}");
//});

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

// If you want to use CorrelationId enrichment
//builder.Host.UseSerilog((context, config) =>
//{
//    config.MinimumLevel.Information()
//        .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
//        .Enrich.WithCorrelationId()
//        .WriteTo.Debug(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {level:u3} {Message:lj}{NewLine}{Exception}")
//        .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {level:u3} {Message:lj}{NewLine}{Exception}");
//});

builder.AddGameStoreDb();

var app = builder.Build();

app.MapGamesEndpoints();
app.MapGenresEndpoints();

app.MigrateDb();

app.UseMiddleware<ApiKeyMiddleware>();
// Apply middleware conditionally to specific path
app.UseWhen(config => config.Request.Path.StartsWithSegments("/api/game"),
    gameApi =>
    {
        gameApi.UseMiddleware<ApiKeyMiddleware>();

    });

app.Map("/api/GetDateTime", async dtApi =>
{
    await dtApi.Response.WriteAsJsonAsync(new
    {
        CurrentDateTime = DateTime.Now
    });
});

app.MapWhen(context => context.Request.Path.StartsWithSegments("/api/DateTime"),
    app1 => app1.Run(async context =>
    {
        Log.Information("Endpoint start");
        await context.Response.WriteAsJsonAsync(new
        {
            CurrentDateTime = DateTime.Now,
            message = "This endpoint provides the current date and time."
        });
        Log.Information($"Endpoint finished {context.Response.StatusCode}");
    }));

app.UseSerilogRequestLogging();
app.Run();
// run db migrations and seed data
// first use namespaces in program.cs
// using GameStore.Data; => to access GameStoreContext
// builder.AddGameStoreDb(); => to seed data
// app.MigrateDb(); => to run migrations
// dotnet ef migrations add InitialCreate --output-dir Data/Migrations
// dotnet ef database update
