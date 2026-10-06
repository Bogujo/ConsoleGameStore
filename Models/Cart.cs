namespace ConsoleGameStore.Models;

public class Cart
{
    public List<CartItem> Items { get; set; } = new();

    public decimal Total => Items.Sum(x => x.Total);
}
