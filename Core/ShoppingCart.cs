namespace Core;

public class ShoppingCart
{
    public Guid Id { get; set; }
    private decimal finalPrice;
    public decimal FinalPrice {
        get {
            return this.CartProducts.Aggregate(0M, (a,b) => a + b.ProductQuantity*b.Product.Price);
        }
        set => finalPrice = value;
    }
    public string? AppUserId { get; set; }
    public AppUser AppUser { get; set; } = default!;
    private int count;
    public int Count {
        get => this.CartProducts.Aggregate(0, (a,b) => a + b.ProductQuantity);

        set => this.count = value;
    }
    public ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();
}
