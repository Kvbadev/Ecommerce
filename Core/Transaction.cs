namespace Core;

public class Transaction
{
    //add transaction status and stuff
    public Guid Id { get; set; }
    public AppUser AppUser { get; set; } = default!;
    public decimal Price { get; set; }
    public ICollection<CountableProduct> Products { get; set; } = 
    new List<CountableProduct>();

    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    public bool Failure { get; set; }
}
