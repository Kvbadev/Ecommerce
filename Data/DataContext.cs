using Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class DataContext : IdentityDbContext<AppUser>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; } = default!;
    public virtual DbSet<AppUser> AppUsers {get; set;} = default!;
}
