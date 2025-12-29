using GameStore.Data;
using GameStore.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

builder.AddGameStoreDb();

var app = builder.Build();

app.MapGamesEndpoints();

app.MigrateDb();

app.Run();
// run db migrations and seed data
// first use namespaces in program.cs
// using GameStore.Data; => to access GameStoreContext
// builder.AddGameStoreDb(); => to seed data
// app.MigrateDb(); => to run migrations
// dotnet ef migrations add InitialCreate --output-dir Data/Migrations
// dotnet ef database update
