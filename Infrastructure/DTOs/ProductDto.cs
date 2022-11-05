namespace Infrastructure.DTOs;

public class ProductDto
{
    public string? Name { get; set; } = default!;
    public string? Description { get; set; } = default!;
    public decimal Price { get; set; }
}
