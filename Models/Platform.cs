using System.ComponentModel.DataAnnotations;

namespace ConsoleGameStore.Models;

public class Platform
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public ICollection<GamePlatform> GamePlatforms { get; set; } = new List<GamePlatform>();
}
