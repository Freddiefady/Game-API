using GameStore.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

var app = builder.Build();

app.MapGamesEndpoints();

app.Run();
// run db migrations and seed data
// dotnet ef migrations add InitialCreate --output-dir Data/Migrations
// dotnet ef database update --project GameStore.Data --startup-project GameStore.Api
