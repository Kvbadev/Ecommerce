using AutoMapper;
using Braintree;
using Core;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Web.Services;
using Web.Services.JwtToken;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IJwtTokenService _tokenService;
    private readonly DataContext _context;
    private readonly IPaymentService _paymentService;
    public PaymentController(IJwtTokenService tokenService, DataContext context, IPaymentService paymentService)
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

    //TODO: improve not to initiate product at request
    [HttpPost("buy/{nonce}")]
    public async Task<IActionResult> Buy([FromServices]IMapper mapper, [FromRoute]string nonce, [FromBody]ProductSimplified? product)
    {
        Result<Braintree.Transaction> res;
        Core.Transaction transaction;

        var user = _context.Users.Include(x => x.Transactions).Include(x => x.ShoppingCart).ThenInclude(x => x.CartProducts).ThenInclude(x => x.Product).FirstOrDefault(x => x.Id == _tokenService.ExtractId());
        if(user == null || user.ShoppingCart.Count == 0)
        {
            return BadRequest("Invalid request");
        }
        if(product==null || product?.Quantity == 0) //buy entire cart
        {
            res = await _paymentService.ProceedTransaction(user.ShoppingCart, nonce);

            // ICollection<Product> products = mapper.Map<CartProduct[], ICollection<Product>>(user.ShoppingCart.CartProducts.ToArray<CartProduct>());

            transaction = new Core.Transaction
            {
                // Products = products,
                Products = user.ShoppingCart.CartProducts.Select(x => x.Product).ToList() ?? new List<Product>(),
                Price = user.ShoppingCart.FinalPrice 
            };
        }
        else //buy only the product provided in the request
        {
            var prod = await _context.Products.FindAsync(product!.Id);
            if(prod == null)
            {
                return BadRequest("Product not found");
            }

            res = await _paymentService.ProceedTransaction(prod, product.Quantity, nonce);

            transaction = new Core.Transaction
            {
                Products = new List<Product>{ prod },
                Price = prod.Price * product.Quantity
            };

        }
        transaction.AppUser = user;
        transaction.Failure = !res.IsSuccess();

        _context.Transactions.Add(transaction);
        var result = await _context.SaveChangesAsync() > 0;

        return result && res.IsSuccess() ? 
            Ok("Transaction has been proceeded"):
            BadRequest("Could not establish a transaction");
    }
}
