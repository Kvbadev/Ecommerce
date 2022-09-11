using Microsoft.AspNetCore.Mvc;
using Web.Services;
using Web.Services.JwtToken;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IJwtTokenService _tokenService;
    public PaymentController(IJwtTokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpGet("charge")]
    public async Task<IActionResult> GetToken([FromServices]IPaymentService paymentService)
    {
        string id = _tokenService.ExtractId();
        if(id == string.Empty)
        {
            return BadRequest("Could not access necessary client property");
        }

        var token = paymentService.GenerateToken(id);
        return Ok(token);
    }
}
