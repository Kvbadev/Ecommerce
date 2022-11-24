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
    public virtual DbSet<CartProduct> CartProducts { get; set; } = default!;

    public virtual DbSet<ShoppingCart> ShoppingCarts {get; set;} = default!;

    public virtual DbSet<Transaction> Transactions { get; set; } = default!;
    public virtual DbSet<Photo> Photos { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>()
            .HasOne(n => n.ShoppingCart)
            .WithOne(n => n.AppUser)
            .HasForeignKey<ShoppingCart>(fk => fk.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<AppUser>()
            .HasMany(n => n.Transactions)
            .WithOne(n => n.AppUser)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Product>()
            .HasMany(n => n.Photos)
            .WithOne(x => x.Product)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CartProduct>()
            .HasKey(cp => new {cp.ProductId, cp.ShoppingCartId});
        builder.Entity<CartProduct>()
            .HasOne(cp => cp.Product)
            .WithMany(p => p.CartProducts)
            .HasForeignKey(fk => fk.ProductId);
        builder.Entity<CartProduct>()
            .HasOne(cp => cp.ShoppingCart)
            .WithMany(p => p.CartProducts)
            .HasForeignKey(fk => fk.ShoppingCartId);

        builder.Entity<Photo>()
            .HasOne(p => p.Product)
            .WithMany(p => p.Photos)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Product>().Property(x => x.Price).HasColumnType("decimal(18,4)"); 
        builder.Entity<Transaction>().Property(x => x.Price).HasColumnType("decimal(18,4)"); 
    }
}
