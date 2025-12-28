using System.ComponentModel.DataAnnotations;

namespace GameStore.Dtos;

public record UpdateGameDto(
    int Id,
    [Required][StringLength(50)] string Title,
    [Required][StringLength(20)]string Description,
    [Range(1, 100)] decimal? Price,
    DateOnly? DateRelease,
    int? GenreId
    );
