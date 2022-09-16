using Braintree;
using Core;

namespace Web.Services;

public interface IPaymentService
{
    string GenerateToken(string id);
    Task<Result<Braintree.Transaction>> ProceedTransaction(ShoppingCart cart, string nonce);
    Task<Result<Braintree.Transaction>> ProceedTransaction(Product product, int quantity, string nonce);
}