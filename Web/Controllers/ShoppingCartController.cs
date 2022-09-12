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

    [HttpPatch("{shouldAdd}")]
    public async Task<IActionResult> AddProducts(ProductToCartDto product, string shouldAdd)
    {
        var selectedProduct = await _context.Products.FindAsync(product.Id);

        if(selectedProduct == null)
        {
            return BadRequest("Product not found");
        }
        ShoppingCart userCart = _context.ShoppingCarts.Include(x => x.CartProducts)
            .FirstOrDefault(x => x.AppUserId == _jwtTokenService.ExtractId())!;
        if(userCart == null)
        {
            return BadRequest("Something goes not as expected");
        }

        var prodInCart = await _context.CartProducts
            .FirstOrDefaultAsync(x => x.ProductId == product.Id && x.ShoppingCartId == userCart.Id);

        if(shouldAdd == "add")
        {
            if(prodInCart != null)
            {
                prodInCart.ProductQuantity += product.Quantity;
            }
            else
            {
                prodInCart = new CartProduct //redefined to use it further so it is never null
                {
                    Product = selectedProduct,
                    ProductQuantity = product.Quantity
                };
               userCart.CartProducts.Add(prodInCart);
            }

            userCart.FinalPrice += (prodInCart.Product.Price * product.Quantity);
            userCart.Count += product.Quantity;
        }
        else if(shouldAdd == "delete")
        {
            if(prodInCart == null)
            {
                return BadRequest("Product not found");
            }
            if(product.Quantity > prodInCart.ProductQuantity)
            {
                return BadRequest("Number too big");
            }
            prodInCart.ProductQuantity -= product.Quantity;
            if(prodInCart.ProductQuantity <= 0)
            {
                userCart.CartProducts.Remove(prodInCart);
            }
            userCart.FinalPrice -= (prodInCart.Product.Price * product.Quantity);
        }
        else
        {
            return BadRequest("Incorrect query argument");
        }

       
        var res = await _context.SaveChangesAsync() > 0;
        if(res)
        {
            return NoContent();
        }
        return BadRequest("Could not add the items");
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        ShoppingCart cart = _context.ShoppingCarts.Include(x => x.CartProducts).FirstOrDefault(x => x.AppUserId == _jwtTokenService.ExtractId())!;
        
        cart.CartProducts = new List<CartProduct>();
        cart.FinalPrice = 0;

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

        // cart.CartProducts = _mapper.Map<newCart.Items
        _mapper.Map<ShoppingCartDto, ShoppingCart>(newCart, cart);
        var res = await _context.SaveChangesAsync() > 0;

        return res ? Ok("New cart has been set") : BadRequest("Could not persist changes in database");
   }

}
