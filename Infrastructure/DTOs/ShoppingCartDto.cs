using Core;

namespace Infrastructure.DTOs;

public class ShoppingCartDto
{
    public int Count { get; set; }
    public ICollection<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
    public decimal Sum { get; set; }
}
