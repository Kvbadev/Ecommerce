using Braintree;

namespace Web.Services;

public class PaymentService : IPaymentService
{
    private readonly IConfiguration _configuration;
    public PaymentService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(string id)
    {
        var gateway = new BraintreeGateway
        {
            Environment = Braintree.Environment.SANDBOX,
            MerchantId = _configuration["Payments:Merchant_Id"],
            PublicKey = _configuration["Payments:Public_Key"],
            PrivateKey = _configuration["Payments:Secret_Key"]
        };

        string token = gateway.ClientToken.Generate();
        return token;
    }
}