namespace Core;

public class ShoppingCart
{
    public Guid Id { get; set; }
    public decimal FinalPrice { get; set; }
    public string? AppUserId { get; set; }
    public AppUser AppUser { get; set; } = default!;
    public int Count { get; set; }
    public ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();
}
