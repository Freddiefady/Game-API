namespace GameStore.Dtos;

public record GameDetailsDto(
    int Id,
    string Title,
    string? Description,
    decimal? Price,
    DateOnly? DateRelease,
    int GenreId
    );
