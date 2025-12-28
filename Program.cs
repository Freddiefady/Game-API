using GameStore.Data;
using GameStore.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddValidation();

app.MapGamesEndpoints();
app.Run();
