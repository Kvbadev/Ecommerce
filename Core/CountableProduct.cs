namespace Core;

public class CountableProduct
{
    public Guid Id { get; set; }
    public Product Product { get; set; } = default!;
    public int Quantity { get; set; }
}