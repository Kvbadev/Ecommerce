using AutoMapper;
using Core;
using Data;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Services.JwtToken;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ShoppingCartController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IMapper _mapper;

    public ShoppingCartController(DataContext context, IJwtTokenService jwtTokenService, IMapper mapper)
    {
        _jwtTokenService = jwtTokenService;
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ShoppingCartDto?> Get() 
    {
        var user = await _context.Users.Include(x => x.ShoppingCart).ThenInclude(x => x.CartProducts)
            .ThenInclude(x => x.Product).FirstOrDefaultAsync(user => user.Id == _jwtTokenService.ExtractId());
        if(user == null)
        {
            return null;
        }

        var cart = new ShoppingCartDto();
        _mapper.Map<ShoppingCart, ShoppingCartDto>(user.ShoppingCart, cart);

        return cart;
    }

    //TODO: improve this functions as it's not very concise
    //Method to add/delete products from user's cart
    //When shouldAdd (that is being obtained by query string) is set to 'Add' then method removes desired quantity of products
    [HttpPatch("{add?}")]
    public async Task<IActionResult> ManageProducts(ProductSimplified product, bool add)
    {
        var user = await _context.Users.Include(x => x.ShoppingCart)
            .ThenInclude(x => x.CartProducts)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == _jwtTokenService.ExtractId());
            
        //additionally checks if user is null by adding ? on dereference
        var prodInCart = user?.ShoppingCart.CartProducts
            .FirstOrDefault(x => x.ProductId == product.Id);
        
        if(user is null)
        {
            return BadRequest("Product has not been found in your shopping cart");
        }

        if(add)
        {
            CartProduct? newProd = null;
            if(prodInCart is not null)
            {
                prodInCart.ProductQuantity += product.Quantity;
            }
            else
            {
                newProd = new CartProduct //redefined to use it further so it is never null
                {
                    Product = _context.Products.Find(product.Id)!,
                    ProductQuantity = product.Quantity
                };
               user!.ShoppingCart.CartProducts.Add(newProd);
            }

            user!.ShoppingCart.FinalPrice += ((newProd ?? prodInCart)!.Product.Price * product.Quantity);
            user.ShoppingCart.Count += product.Quantity;
        }

        else if(add is false)
        {
            if(product.Quantity > prodInCart!.ProductQuantity)
            {
                return BadRequest("Number is too big");
            }

            prodInCart.ProductQuantity -= product.Quantity;
            if(prodInCart.ProductQuantity <= 0)
            {
                user!.ShoppingCart.CartProducts.Remove(prodInCart);
            }

            user!.ShoppingCart.FinalPrice -= (prodInCart.Product.Price * product.Quantity);
            user.ShoppingCart.Count -= product.Quantity;
        }
        else
        {
            return BadRequest("Incorrect query argument");
        }

       
        var res = await _context.SaveChangesAsync() > 0;
        if(res)
        {
            return Ok("Changes've been persisted");
        }

        return BadRequest("Could not add/remove item(s)");
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        ShoppingCart cart = _context.ShoppingCarts.Include(x => x.CartProducts).FirstOrDefault(x => x.AppUserId == _jwtTokenService.ExtractId())!;
        
        cart.CartProducts = new List<CartProduct>();
        cart.FinalPrice = 0;
        cart.Count = 0;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> SetNewCart(ShoppingCartDto newCart)
    {
        ShoppingCart cart = _context.ShoppingCarts.Include(x => x.CartProducts).FirstOrDefault(x => x.AppUserId == _jwtTokenService.ExtractId())!;
        if(cart == null)
        {
            return BadRequest("This user does not exist");
        }
        var isValid = await VerifyCart(newCart);
        if(isValid == false)
        {
            return BadRequest("Shopping cart supplied by the was invalid");
        }

        _mapper.Map<ShoppingCartDto, ShoppingCart>(newCart, cart);
        var res = await _context.SaveChangesAsync() > 0;

        return res ? Ok("New cart has been set") : BadRequest("Could not persist changes in database");
   }

   private async Task<bool> VerifyCart(ShoppingCartDto cart)
   {
    var products = await _context.Products.ToListAsync();
    if(cart.Count <= 0){
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
