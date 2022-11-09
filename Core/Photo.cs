namespace Core;
public class Photo
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public Product? Product { get; set; }
}