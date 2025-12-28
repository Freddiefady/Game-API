using GameStore.Dtos;
using GameStore.Models;

namespace GameStore.Endpoints;

public static class GameEndpoint
{
    const string GetGameEndpointName = "/games";
    
    private static readonly List<GameDto> games = [
        new GameDto(1, "The Witcher 3: Wild Hunt", "An open-world RPG set in a fantasy universe.", 39.99m, new DateOnly(2015, 5, 19)),
        new GameDto(2, "Cyberpunk 2077", "A futuristic open-world RPG set in Night City.", 59.99m, new DateOnly(2020, 12, 10)),
        new GameDto(3, "Red Dead Redemption 2", "An epic Western-themed action-adventure game.", 49.99m, new DateOnly(2018, 10, 26))
    ];
    
    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup(GetGameEndpointName);
        
        // GET /game
        group.MapGet("/", () => games);
        
        // GET /game/{id}
        group.MapGet("/{id}", (int id) =>
        {
            var game = games.Find(game => game.Id == id);
            return game is not null ? Results.Ok(game) : Results.NotFound();
        }).WithName(GetGameEndpointName);
        
        // POST /game
        group.MapPost("/", (CreateGameDto newGame) =>
        {
            GameDto game = new(
                Id: games.Count + 1,
                Title: newGame.Title,
                Description: newGame.Description,
                Price: newGame.Price,
                DateRelease: newGame.DateRelease
            );
            
            games.Add(game);
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        // PUT /game/{id}
        group.MapPut(GetGameEndpointName, (int id, UpdateGameDto updateGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);
            if (index == -1)
            {
                return Results.NotFound();
            }
            
            games[index] = new GameDto(
                id,
                updateGame.Title,
                updateGame.Description,
                updateGame.Price,
                updateGame.DateRelease
            );
            
            return Results.NoContent();
        });
        
        // DELETE /game/{id}
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);
            return Results.NoContent();
        });
    }
}