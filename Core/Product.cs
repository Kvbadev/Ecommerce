namespace Core;

public class Product
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public ICollection<CartProduct> CartProducts { get; set; } = default!;
}
