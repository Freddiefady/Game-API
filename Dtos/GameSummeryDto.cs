namespace GameStore.Dtos;

// A DTO is a contract between the client and the server.
// a shared agreement about how data will be transfer.
public record GameSummeryDto(
    int Id,
    string Title,
    string? Description,
    decimal? Price,
    DateOnly? DateRelease,
    string Genre
    );
