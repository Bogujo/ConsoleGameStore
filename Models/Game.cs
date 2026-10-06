using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleGameStore.Models;

public class Game
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Range(0, 1000000)]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    public int GenreId { get; set; }
    public Genre Genre { get; set; } = null!;

    public ICollection<GamePlatform> GamePlatforms { get; set; } = new List<GamePlatform>();
    public ICollection<GamePublisher> GamePublishers { get; set; } = new List<GamePublisher>();
}
