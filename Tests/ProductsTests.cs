namespace Tests;
using Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Data;
using Moq;
using Core;
using Microsoft.EntityFrameworkCore;

public class ProductsTests
{

    [Fact]
    public async Task GetProducts_returns_products_list_and_check_distinct_product()
    {
        var data = new List<Product>
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
            }
        };
        //Arrange
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "ProductsDatabase")
            .Options;

        //Act
        using (var context = new DataContext(options))
        {
            context.AddRange(data);
            context.SaveChanges();


            var testController = new ProductsController(context);

            var productsList = await testController.GetProducts();

            var distinctProduct = await testController.GetProduct(productsList[0].Id);

            //Assert
            Assert.IsType<List<Product>>(productsList);
            Assert.Equal(3, productsList.Count);

            Assert.IsType<Product>(distinctProduct);
            Assert.NotNull(distinctProduct);
        }

    }
}