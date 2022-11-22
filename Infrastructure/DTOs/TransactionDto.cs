using Core;

namespace Infrastructure.DTOs;

public class TransactionDto
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public IEnumerable<ShoppingCartItem> Products { get; set; } = default!;
    public DateTime IssuedAt { get; set; }
    public bool Success { get; set; }
}