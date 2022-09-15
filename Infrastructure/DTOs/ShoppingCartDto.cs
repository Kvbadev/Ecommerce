using Core;

namespace Infrastructure.DTOs;

public class ShoppingCartDto
{
    public int Count { get; set; }
    public ICollection<ProductSimplified> Items { get; set; } = new List<ProductSimplified>();
    public decimal Sum { get; set; }
}
