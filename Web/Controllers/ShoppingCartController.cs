using AutoMapper;
using Core;
using Data;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Services.JwtToken;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<IEnumerable<ProductToCartDto>?> Get() 
    {
        var user = await _context.Users.Include(x => x.ShoppingCart).FirstOrDefaultAsync(user => user.Id == _jwtTokenService.ExtractId());
        if(user == null)
        {
            return Enumerable.Empty<ProductToCartDto>();
        }

        List<ProductToCartDto> items = _mapper.Map<CartProduct[], List<ProductToCartDto>>(await _context.CartProducts.Where(x => x.ShoppingCartId == user.ShoppingCart.Id).Include(x => x.Product).ToArrayAsync());

        return items;
    }

    [HttpPatch("{shouldAdd}")]
    public async Task<IActionResult> AddProducts(ProductToCartDto product, string shouldAdd)
    {
        
        var user = await _context.Users.Include(x => x.ShoppingCart).ThenInclude(x => x.CartProducts).FirstOrDefaultAsync(user => user.Id == _jwtTokenService.ExtractId());
        if(user == null)
        {
            return BadRequest("User not found");
        }

        var productToAdd = await _context.Products.FindAsync(product.Id);
        if(productToAdd == null)
        {
            return BadRequest("Product not found");
        }
        var prodInCart = user.ShoppingCart.CartProducts.FirstOrDefault(x => x.ProductId == product.Id);

        if(shouldAdd == "add")
        {
            if(prodInCart != null)
            {
                prodInCart.ProductQuantity += product.Quantity;
            }
            else
            {
                var cartProduct = new CartProduct
                {
                    Product = productToAdd,
                    ProductQuantity = product.Quantity
                };
                user.ShoppingCart.CartProducts.Add(cartProduct);
            }

            user.ShoppingCart.FinalPrice += (product.Price * product.Quantity);
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
                user.ShoppingCart.CartProducts.Remove(prodInCart);
            }
            user.ShoppingCart.FinalPrice -= (product.Price * product.Quantity);
        }
        else
        {
            return BadRequest("Incorrect query argument");
        }

       
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProducts(ProductToCartDto product)
    {
        var user = await _context.Users.Include(x => x.ShoppingCart).ThenInclude(x => x.CartProducts).FirstOrDefaultAsync(user => user.Id == _jwtTokenService.ExtractId());
        if(user == null)
        {
            return BadRequest("User not found");
        }
        
        await _context.SaveChangesAsync();
        return Ok();
    }
}
