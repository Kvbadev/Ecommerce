using Braintree;
using Core;

namespace Web.Services;

public interface IPaymentService
{
    string GenerateToken(string id);
    Task<Result<Braintree.Transaction>> ProceedTransaction(ShoppingCart cart,
        string nonce, string devData);
    Task<Result<Braintree.Transaction>> ProceedTransaction(Product product,
        int quantity, string nonce, string devData);
}