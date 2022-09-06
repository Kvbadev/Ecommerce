using Core;

namespace Infrastructure.DTOs;

public class CartDto
{
    public ICollection<ProductToCartDto> Products { get; set; } = default!;
    public decimal FinalPrice { get; set; }
}