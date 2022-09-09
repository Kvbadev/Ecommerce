namespace Core;

public class CartProduct
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;
    public Guid ShoppingCartId { get; set; }
    public ShoppingCart ShoppingCart { get; set; } = default!;
    public int ProductQuantity { get; set; }
}
