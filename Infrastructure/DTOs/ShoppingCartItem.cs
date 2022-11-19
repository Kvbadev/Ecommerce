namespace Infrastructure.DTOs;

public class ShoppingCartItem
{
    public ProductDto Product { get; set; } = default!;
    public int Quantity { get; set; }
}
