using Core;

namespace Infrastructure.DTOs;

public class ShoppingCartDto
{
    public int Count { get; set; }
    public ICollection<ProductToCartDto> Items { get; set; } = new List<ProductToCartDto>();
    public decimal Sum { get; set; }
}
