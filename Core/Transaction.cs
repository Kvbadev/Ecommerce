namespace Core;

public class Transaction
{
    public Guid Id { get; set; }
    public AppUser AppUser { get; set; } = default!;
    public decimal Price { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    public bool Failure { get; set; }
}
