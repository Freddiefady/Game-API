namespace GameStore.Models;

public class Game
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public DateOnly DateRelease { get; set; }
    public int GenreId { get; set; }
    public Genre? Genre { get; set; }
}