using System.ComponentModel.DataAnnotations;

namespace ConsoleGameStore.Models;

public class Publisher
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    public ICollection<GamePublisher> GamePublishers { get; set; } = new List<GamePublisher>();
}
