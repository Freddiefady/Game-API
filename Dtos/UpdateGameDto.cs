using System.ComponentModel.DataAnnotations;

namespace GameStore.Dtos;

public record UpdateGameDto(
    int Id,
    [Required][MaxLength(50)] string Title,
    [Required][MaxLength(20)]string? Description,
    decimal? Price,
    DateOnly? DateRelease,
    int? GenreId
    );