using System.Text.Json.Serialization;

namespace Core;

public class ShoppingCart
{
    public Guid Id { get; set; }
    public string? AppUserId { get; set; }
    public AppUser AppUser { get; set; } = default!;
    public ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();

    [JsonIgnore]
    public int Count => this.CartProducts.Aggregate(0, (a,b) => a + b.ProductQuantity);
    [JsonIgnore]
    public decimal FinalPrice => this.CartProducts.Aggregate(0M, (a,b) => a + b.ProductQuantity*b.Product.Price);
}
