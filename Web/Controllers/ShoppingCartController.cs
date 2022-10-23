using AutoMapper;
using Core;
using Data;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Services;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ShoppingCartController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IMapper _mapper;

    public ShoppingCartController(DataContext context, IJwtTokenService jwtTokenService,
    IMapper mapper)
    {
        _jwtTokenService = jwtTokenService;
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ShoppingCartDto?> Get() 
    {
        var user = await _context.Users.Include(x => x.ShoppingCart).ThenInclude(x => x.CartProducts)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(user => user.Id == _jwtTokenService.ExtractId());

        if(user == null)
        {
            return null;
        }

        var cart = new ShoppingCartDto();
        _mapper.Map<ShoppingCart, ShoppingCartDto>(user.ShoppingCart, cart);

        return cart;
    }

    //Method to add/delete products from user's cart
    [HttpPatch("update")]
    public async Task<IActionResult> ManageProducts(ProductSimplified product)
    {
        var user = await _context.Users.Include(x => x.ShoppingCart)
            .ThenInclude(x => x.CartProducts)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == _jwtTokenService.ExtractId());
            
        //additionally checks if user in null by adding ? on dereference
        var prodInCart = user?.ShoppingCart.CartProducts
            .FirstOrDefault(x => x.ProductId == product.Id);
        
        if(user is null)
        {
            return BadRequest("User has not been found");
        }

        if(prodInCart is not null)
        {
          
            prodInCart.ProductQuantity += product.Quantity;

            if(prodInCart.ProductQuantity <= 0)
            {
                user.ShoppingCart.CartProducts.Remove(prodInCart);
            }
        }
        else
        {
            var newProd = new CartProduct //redefined to use it further so it is never null
            {
                Product = _context.Products.Find(product.Id)!,
                ProductQuantity = product.Quantity
            };
            user.ShoppingCart.CartProducts.Add(newProd);
        }

        user.ShoppingCart.FinalPrice = CalculatePrice(user.ShoppingCart);
        user.ShoppingCart.Count = CalculateCount(user.ShoppingCart);
       
        var res = await _context.SaveChangesAsync() > 0;

        return res ?
            Ok("Changes've been persisted") :
            BadRequest("Could not add/remove item(s)");
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        ShoppingCart cart = _context.ShoppingCarts.Include(x => x.CartProducts)
            .FirstOrDefault(x => x.AppUserId == _jwtTokenService.ExtractId())!;
        
        cart.CartProducts = new List<CartProduct>();
        cart.FinalPrice = 0;
        cart.Count = 0;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> SetNewCart(ShoppingCartDto newCart)
    {
        ShoppingCart cart = _context.ShoppingCarts.Include(x => x.CartProducts)
            .FirstOrDefault(x => x.AppUserId == _jwtTokenService.ExtractId())!;
        if(cart == null)
        {
            return BadRequest("This user does not exist");
        }
        var isValid = (await VerifyCart(newCart) && isTheSame(newCart, cart));
        if(isValid == false)
        {
            return BadRequest("Shopping cart supplied by the client was invalid");
        }

        _mapper.Map<ShoppingCartDto, ShoppingCart>(newCart, cart);
        var res = await _context.SaveChangesAsync() > 0;

        return res ? Ok("New cart has been set") : 
        BadRequest("Could not persist changes in database");
   }


    private int CalculateCount(ShoppingCart cart)
    {
        int count = 0;
        foreach(var prod in cart.CartProducts)
        {
            count += prod.ProductQuantity;
        }
        return count;
    }

    private decimal CalculatePrice(ShoppingCart cart)
    {
        decimal price = 0M;
        foreach(var prod in cart.CartProducts)
        {
            price += prod.ProductQuantity * prod.Product.Price;
        }
        return price;
    }

    private bool isTheSame(ShoppingCartDto cart, ShoppingCart actual)
    {
        var prods = _mapper.Map<CartProduct[], IEnumerable<ProductSimplified>>(actual.CartProducts.ToArray());
        foreach(var it in cart.Items)
        {
            var tmp = prods.FirstOrDefault(x => x.Id == it.Id); 
            if(tmp != null && tmp.Quantity == it.Quantity) return false;
        }
        return true;
    }
    private async Task<bool> VerifyCart(ShoppingCartDto cart)
    {
        var products = await _context.Products.ToListAsync();
        if(cart.Count < 0){
            return false;
        }
        decimal sum = 0;
        int count = 0;
        foreach(var item in cart.Items)
        {
            var prod = products.Find(x => x.Id == item.Id);
            if(prod == null || item.Quantity == 0)
            {
                return false;
            }
            count += item.Quantity;
            sum += prod.Price * item.Quantity;
        }

        if(sum != cart.Sum || count != cart.Count)
        {
            return false;
        }
        return true;
    }
}
