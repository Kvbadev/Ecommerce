using Braintree;
using Data;
using Microsoft.AspNetCore.Mvc;
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

    [HttpPost("transaction")]
    public async Task<IActionResult> CreateTransaction([FromBody]string nonce)
    {
        var user = _context.Users.Include(x => x.ShoppingCart).FirstOrDefault(x => x.Id == _tokenService.ExtractId());
        if(user == null)
        {
            return BadRequest("Invalid user");
        }
        var res = await _paymentService.ProceedTransaction(user.ShoppingCart, nonce, user.UserName);
        if(res.IsSuccess())
        {
            return Ok("Transaction has been established");
        }
        return BadRequest("Could not establish a transaction");
    }
}
