namespace Core;
using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
    public string Firstname { get; set; } = default!;
    public string Lastname { get; set; } = default!;
    public DateTime CreationDate = DateTime.UtcNow;
    public ShoppingCart ShoppingCart { get; set; } = default!;
    public ICollection<Transaction> Transactions { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public DateTime RefreshTokenExpiry { get; set; }
}
