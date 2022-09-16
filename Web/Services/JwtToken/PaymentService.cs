using Braintree;
using Core;
using Infrastructure.DTOs;

namespace Web.Services;

public class PaymentService : IPaymentService
{
    private readonly IConfiguration _configuration;
    private readonly BraintreeGateway _gateway;
    public PaymentService(IConfiguration configuration)
    {
        _configuration = configuration;
        _gateway = new BraintreeGateway
        {
            Environment = Braintree.Environment.SANDBOX,
            MerchantId = _configuration["Payments:Merchant_Id"],
            PublicKey = _configuration["Payments:Public_Key"],
            PrivateKey = _configuration["Payments:Secret_Key"]
        };
    }

    public string GenerateToken(string id)
    {
        string token = _gateway.ClientToken.Generate();
        return token;
    }

    public async Task<Result<Braintree.Transaction>> ProceedTransaction(ShoppingCart cart, string nonce)
    {
        var request = new TransactionRequest
        {
            Amount = cart.FinalPrice,
            PaymentMethodNonce = nonce,
            DeviceData = string.Empty,
            Options = new TransactionOptionsRequest
            {
                SubmitForSettlement = true
            }
        };
        var result = await _gateway.Transaction.SaleAsync(request);
        return result;
    }

    public async Task<Result<Braintree.Transaction>> ProceedTransaction(Product product, int quantity, string nonce)
    {
        var request = new TransactionRequest
        {
            Amount = product.Price * quantity,
            PaymentMethodNonce = nonce,
            DeviceData = string.Empty,
            Options = new TransactionOptionsRequest
            {
                SubmitForSettlement = true
            }
        };
        var result = await _gateway.Transaction.SaleAsync(request);
        return result;
    }
}