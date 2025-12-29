using GameStore.Data;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Endpoints;

public static class GenreEndpoint
{
    public static void MapGenresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/genres");

        group.MapGet("/", async (GameStoreContext db) =>
        {
            var genres = await db.Genres
                .Select(g => new Dtos.GenresDto(g.Id, g.Name))
                .AsNoTracking()
                .ToListAsync();

            return Results.Ok(genres);
        })
        .WithTags("Genres")
        .WithDescription("Get all genres");
    }
}
