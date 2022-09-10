using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    [HttpGet("charge")]
    public async Task<IActionResult> Test()
    {
        return Ok();
    }
}
