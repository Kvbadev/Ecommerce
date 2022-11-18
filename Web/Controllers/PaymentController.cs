using AutoMapper;
using Braintree;
using Core;
using Data;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Services;

namespace Web.Controllers;

public class PaymentController : DefaultController
{
    private readonly IJwtTokenService _tokenService;
    private readonly DataContext _context;
    private readonly IPaymentService _paymentService;
    public PaymentController(IJwtTokenService tokenService, DataContext context,
    IPaymentService paymentService)
    {
        _paymentService = paymentService; 
        _context = context;
        _tokenService = tokenService;
    }

    [HttpGet("token")]
    public IActionResult GetToken()
    {
        string id = _tokenService.ExtractId();
        if(id == string.Empty)
        {
            return BadRequest("Could not access necessary client property");
        }

        var token = _paymentService.GenerateToken(id);
        return Ok(token);
    }

    [HttpPost("price")]
    public async Task<IActionResult> GetPrice(ProductSimplified? prod)
    {
        if(prod?.Id == Guid.Empty)
        {
            var cart = await _context.ShoppingCarts
                .FirstOrDefaultAsync(x => x.AppUserId==_tokenService.ExtractId());
            return cart != null ? Ok(cart.FinalPrice) : BadRequest("User not found");
        }
        var product = await _context.Products.FindAsync(prod!.Id);
        if(product is null || prod.Quantity <= 0)
        {
            return BadRequest("Invalid quantity or Id");
        }
        return Ok(prod.Quantity * product.Price);
    }

    [HttpPost("buy")] //example: buy?nonce=test&id=id&quantity=quantity
    public async Task<IActionResult> Buy([FromServices]IMapper mapper, string nonce,
        Guid id, int quantity, [FromBody]string devData,
        [FromServices]IValidator<Core.Transaction> validator)
    {
        Result<Braintree.Transaction> res;
        Core.Transaction transaction;

        var user = _context.Users.Include(x => x.Transactions)
            .Include(x => x.ShoppingCart).ThenInclude(x => x.CartProducts)
            .ThenInclude(x => x.Product).FirstOrDefault(x => x.Id == _tokenService.ExtractId());

        if(user == null || (user.ShoppingCart.Count == 0 && quantity==0 && id != Guid.Empty))
        {
            return BadRequest("Invalid request");
        }

        if(quantity == 0) //buy entire cart
        {
            res = await _paymentService.ProceedTransaction(user.ShoppingCart, nonce, devData);

            transaction = new Core.Transaction
            {
                Products = new List<CountableProduct>(
                    mapper.Map<CartProduct[], ICollection<CountableProduct>>
                        (user.ShoppingCart.CartProducts.ToArray())
                ),
                Price = user.ShoppingCart.FinalPrice,
                Failure = !res.IsSuccess()
            };
        }
        else //buy only the product provided in the request
        {
            var prod = await _context.Products.FindAsync(id);
            if(prod == null)
            {
                return BadRequest("Product not found");
            }

            var localProduct = new CountableProduct //product to add to transaction
            {
                Product = prod,
                Quantity = quantity
            };

            res = await _paymentService.ProceedTransaction(prod, quantity, nonce, devData);

            transaction = new Core.Transaction
            {
                Products = new List<CountableProduct>(1){localProduct},
                Price = prod.Price * quantity,
                Failure = !res.IsSuccess()
            };

        }
        transaction.AppUser = user; //is it necessary?

        var message = await ValidateEntity(transaction);

        if(message != string.Empty)
        {
            return BadRequest(message);
        }

        _context.Transactions.Add(transaction);
        var result = await _context.SaveChangesAsync() > 0;

        return result && res.IsSuccess() ? 
            Ok("Transaction has been proceeded"):
            BadRequest("Could not establish a transaction");
    }
}
