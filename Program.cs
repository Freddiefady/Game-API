using GameStore.Data;
using GameStore.Endpoints;
using GameStore.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

builder.AddGameStoreDb();

var app = builder.Build();

app.MapGamesEndpoints();
app.MapGenresEndpoints();

app.MigrateDb();

app.UseMiddleware<ApiKeyMiddleware>();

app.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0); // Set the default API version to 1.0 if a version is not specified in the request.
    config.AssumeDefaultVersionWhenUnspecified = true; // Assume default version when not specified
    config.ReportApiVersions = true; // To Let clients know which API versions are supported
    config.ApiVersionReader = new HeaderApiVersionReader("api-version"); // To pass the version information in the request header.
});

app.Run();
// run db migrations and seed data
// first use namespaces in program.cs
// using GameStore.Data; => to access GameStoreContext
// builder.AddGameStoreDb(); => to seed data
// app.MigrateDb(); => to run migrations
// dotnet ef migrations add InitialCreate --output-dir Data/Migrations
// dotnet ef database update
