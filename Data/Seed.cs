using Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public class Seed
{
    public static async Task SeedData(IServiceProvider serviceProvider, IEnumerable<string> photos)
    {
        var context = serviceProvider.GetRequiredService<DataContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        //automatic migration for sqlite database to not throw table doesn't exist exception
        try{
        await context.Database.MigrateAsync();
        } catch (Exception e) {
            if(!e.Message.Contains("already exists")) {
                throw;
            }
        }
        
        if(await roleManager.RoleExistsAsync("Administrator") is false)
        {
            await roleManager.CreateAsync(new IdentityRole("Administrator"));
        }
        if(await userManager.FindByNameAsync("Administrator") == null)
        {
            var outcome = await userManager.CreateAsync(new AppUser("Administrator"), "Pa$$w0rd");
            if(outcome.Succeeded)
            {
                var user = context.Users.FirstOrDefault(x => x.UserName=="Administrator")!;
                await userManager.AddToRoleAsync(user,"Administrator");
                            
            }

        }
        if(await roleManager.RoleExistsAsync("User") is false)
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
        }

        var toAppend = new List<Product>
        {
            new Product
            {
                Name = "Portable Water Cup",
                Description = "Impeccable, no water is spilled if I put it on head, plus it fits perfect in the side pocket of the backpack. He arrived in a couple of weeks, all right.",
                Price = 15.50M,
               
            },
            new Product
            {
                Name = "Plasma Lighter",
                Description = "Really portable, windproof and most importantly reachargable usb lighter is going to be perfect if You smoke or want to start",
                Price = 20.75M,
              
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
        //instead of hardcoding urls as this approach is faster, though prone to errors
        foreach(var e in toAppend) 
        {
            e.Photos = new List<Photo>(photos.Where(x => x
                .Contains(e.Name.Replace(' ', '_').ToLower()))
                .Select(val => 
                {
                    return new Photo
                    {
                        Url = val,
                    };
                }));
            
            if(context.Products.FirstOrDefault(x => x.Name == e.Name) == null)
            {
                context.Products.Add(e);
            }
        }

        var res = await context.SaveChangesAsync();
    }
}
