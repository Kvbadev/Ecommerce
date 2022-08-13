using Core;

namespace Data;

public class Seed
{
    public static async Task SeedData(DataContext context)
    {
        if(context.Products.Any())
        {
            return;
        }
        var toAppend = new List<Product>
        {
            new Product
            {
                Name = "AM:PM Watch",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum",
                Price = 129.99M
            },
            new Product
            {
                Name = "G-Shock Watch",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum",
                Price = 100.50M
            },
            new Product
            {
                Name = "Rolex Watch",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum",
                Price = 10100.90M
            }
        };
        await context.AddRangeAsync(toAppend);
        await context.SaveChangesAsync();
    }
}
