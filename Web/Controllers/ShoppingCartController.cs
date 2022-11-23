using AutoMapper;
using Core;
using Data;
using FluentValidation;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Services;

namespace Web.Controllers;
public class ShoppingCartController : DefaultController
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
            .ThenInclude(x => x.Product).ThenInclude(x => x.Photos)
            .FirstOrDefaultAsync(user => user.Id == _jwtTokenService.ExtractId());

        if(user == null)
        {
            return null;
        }
        return _mapper.Map<ShoppingCart, ShoppingCartDto>(user.ShoppingCart);
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

            var message = await ValidateEntity<CartProduct>(newProd);
            if(message != string.Empty)
            {
                return BadRequest(message);
            }

            user.ShoppingCart.CartProducts.Add(newProd);
        }

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
        
        if(cart.Count == 0) return Ok();
        
        cart.CartProducts = new List<CartProduct>();

        var res = await _context.SaveChangesAsync();

        return res > 0 ? Ok() : BadRequest("Could not persist changes");
    }

    [HttpPost]
    public async Task<IActionResult> SetNewCart(ShoppingCartDto newCart)
    {
        var user = _context.Users.Include(x => x.ShoppingCart).ThenInclude(x => x.CartProducts)
            .ThenInclude(x => x.Product).FirstOrDefault(x => x.Id == _jwtTokenService.ExtractId());

        if(user == null)
        {
            return BadRequest("This user does not exist");
        }
        var isValid = (await VerifyCart(newCart) && !isTheSame(newCart, user.ShoppingCart));
        if(isValid == false)
        {
            return BadRequest("Shopping cart supplied by the client was invalid");
        }

        _mapper.Map(newCart, user.ShoppingCart);

        var res = await _context.SaveChangesAsync() > 0;

        return res ? Ok("New cart has been set") : 
        BadRequest("Could not persist changes in database");
   }
    private bool isTheSame(ShoppingCartDto cart, ShoppingCart actual)
    {
        // var prods = _mapper.Map<CartProduct[], IEnumerable<ProductSimplified>>(actual.CartProducts.ToArray());
        foreach(var it in actual.CartProducts)
        {
            var tmp = cart.Items.FirstOrDefault(x => x.Product.Id == it.ProductId);
            if(tmp == null || tmp.Quantity != it.ProductQuantity) return false;
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
            var prod = products.Find(x => x.Id == item.Product.Id);
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
