namespace Core;
using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
    public AppUser()
    {
    }

    public AppUser(string userName) : base(userName)
    {
    }

    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public  DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public ShoppingCart ShoppingCart { get; set; } = default!;
    public ICollection<Transaction> Transactions { get; set; } = default!;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiry { get; set; }
}
