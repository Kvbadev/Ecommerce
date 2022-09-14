using Braintree;
using Core;

namespace Web.Services;

public interface IPaymentService
{
    string GenerateToken(string id);
    Task<Result<Transaction>> ProceedTransaction(ShoppingCart cart, string nonce, string DeviceData);
}