namespace GameStore.Dtos;

public record GameDto(
    int Id,
    string Title,
    string? Description,
    decimal? Price,
    DateOnly? DateRelease
    );