namespace Core;
using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
    public string Firstname { get; set; } = default!;
    public string Lastname { get; set; } = default!;
    public override string UserName
    {
        set {
            if(this.UserName == string.Empty) 
                throw new InvalidOperationException("Value already set");
            this.UserName = value;
        }
    }
    public DateTime CreationDate = DateTime.UtcNow;
    public ShoppingCart ShoppingCart { get; set; } = default!;
    public ICollection<Transaction> Transactions { get; set; } = default!;
}
