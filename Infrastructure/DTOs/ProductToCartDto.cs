using Core;

namespace Infrastructure.DTOs;

public class ProductToCartDto
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public decimal? Price { get; set; }
}