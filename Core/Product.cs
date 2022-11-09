using System.Text.Json.Serialization;

namespace Core;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public ICollection<Photo> Photos {get; set;} = default!;

    [JsonIgnore]
    public ICollection<CartProduct> CartProducts { get; set; } = default!;
}
