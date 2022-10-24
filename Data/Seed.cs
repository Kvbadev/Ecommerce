using Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public class Seed
{
    public static async Task SeedData(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<DataContext>();
        // var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        // var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // await roleManager.CreateAsync(new IdentityRole("Administrator"));
        // await roleManager.CreateAsync(new IdentityRole("User"));

        // var res = await userManager.AddToRoleAsync(context.Users
        //                 .FirstOrDefault(x => x.UserName=="jakub"), "Administrator");

        if(context.Products.Any())
        {
            return;
        }

        //testing roles

        var toAppend = new List<Product>
        {
            new Product
            {
                Name = "Portable Water Cup",
                Description = "Impeccable, no water is spilled if I put it on head, plus it fits perfect in the side pocket of the backpack. He arrived in a couple of weeks, all right.",
                Price = 15.50M
            },
            new Product
            {
                Name = "Plasma Lighter",
                Description = "Really portable, windproof and most importantly reachargable usb lighter is going to be perfect if You smoke or want to start",
                Price = 20.75M
            },
            new Product
            {
                Name = "Air Humidifier",
                Description = "Very cute but works if with direct usb power, it is not rechargeable in electronic aspects, but if you can change the humidifier bulb",
                Price = 9.99M
            },
            new Product
            {
                Name = "Alarm Clock",
                Description = "The watch is cool. Multifunctional. They work from 3 Pinky batteries, or from a USB wire. Adjustable brightness, there are 3 alarm clocks. Shipping fast. It's especially pleasant that I managed to order before price growth. In the settings sorted out the video from yutuba. Very satisfied with the goods. Thank you seller",
                Price = 30.00M
            },
            new Product
            {
                Name = "Military Thermos",
                Description = "Delivery in 6 days from Russia to Moscow. Very reliable and convenient thermos. Heat holds for a long time. Reliable Metal cork-non-spillage. Comfortable handle + shoulder strap. Mark quality control passed. The seller deserves great respect.",
                Price = 12.50M
            },
            new Product
            {
                Name = "Bluetooth Speaker",
                Description = "Great column! Sound WoW, which was not particularly expected, but in vain! Delivery to Russia before the specified period for two weeks! In the kit two wires! I advise you to buy!",
                Price = 83.99M
            },
            new Product
            {
                Name = "Wall Clock",
                Description = "The wall decor clock comes with self-adhesive numbers and auxiliary scale ruler, also there is an installation video for your reference. So one can easily to install and enjoy the DIY fun of making your own unique house decorations for living room.",
                Price = 45.00M
            }
        };
        await context.AddRangeAsync(toAppend);
        await context.SaveChangesAsync();
    }
}
