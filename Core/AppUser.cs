namespace Core;
using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public DateTime CreationDate = DateTime.UtcNow;
    public ShoppingCart ShoppingCart { get; set; } = default!;
}
