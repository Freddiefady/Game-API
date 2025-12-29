using GameStore.Data;
using GameStore.Dtos;
using GameStore.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Endpoints;

public static class GameEndpoint
{
    const string GetGameEndpointName = "/games";

    // private static readonly List<GameSummeryDto> games = [
    //     new GameDto(1, "The Witcher 3: Wild Hunt", "An open-world RPG set in a fantasy universe.", 39.99m, new DateOnly(2015, 5, 19)),
    //     new GameDto(2, "Cyberpunk 2077", "A futuristic open-world RPG set in Night City.", 59.99m, new DateOnly(2020, 12, 10)),
    //     new GameDto(3, "Red Dead Redemption 2", "An epic Western-themed action-adventure game.", 49.99m, new DateOnly(2018, 10, 26))
    // ];

    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup(GetGameEndpointName);

        // GET /game
        group.MapGet("/", async (GameStoreContext dbcontext) => await dbcontext.Games
            .Include(game => game.Genre)
            .Select(game => new GameSummeryDto(
                    game.Id,
                    game.Title,
                    game.Description,
                    game.Price,
                    game.DateRelease,
                    game.Genre!.Name
                ))
                .AsNoTracking()
                .ToListAsync()
        );

        // GET /game/{id}
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);
            return game is not null ? Results.Ok(
                new GameDetailsDto(
                    game.Id,
                    game.Title,
                    game.Description,
                    game.Price,
                    game.DateRelease,
                    game.GenreId
                )
            ) : Results.NotFound();
        }).WithName(GetGameEndpointName);

        // POST /game
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = new()
            {
                Title = newGame.Title,
                Description = newGame.Description,
                Price = newGame.Price,
                DateRelease = newGame.DateRelease,
                GenreId = newGame.GenreId
            };

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            GameDetailsDto gameDto = new(
                game.Id,
                game.Title,
                game.Description,
                game.Price,
                game.DateRelease,
                game.GenreId
            );

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, gameDto);
        });

        // PUT /game/{id}
        group.MapPut("/{id}", async (
            int id,
            UpdateGameDto updateGame,
            GameStoreContext dbContext
            ) => {
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            existingGame.Title = updateGame.Title;
            existingGame.Description = updateGame.Description;
            existingGame.Price = updateGame.Price ?? existingGame.Price;
            existingGame.DateRelease = updateGame.DateRelease ?? existingGame.DateRelease;
            existingGame.GenreId = updateGame.GenreId ?? existingGame.GenreId;

            dbContext.Games.Update(existingGame);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE /game/{id}
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games

            .FindAsync(id);
            if (existingGame is null)
            {
                return Results.NotFound();
            }

            await dbContext.Games
            .Where(game => game.Id == id)
            .ExecuteDeleteAsync();

            return Results.NoContent();
        });
    }
}
