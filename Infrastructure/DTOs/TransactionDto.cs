using Core;

namespace Infrastructure.DTOs;

public class TransactionDto
{
    public decimal Price { get; set; }
    public IEnumerable<ProductSimplified> Products { get; set; } = default!;
    public DateTime IssuedAt { get; set; }
    public bool Success { get; set; }
}