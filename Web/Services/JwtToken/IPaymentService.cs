namespace Web.Services;

public interface IPaymentService
{
    string GenerateToken(string id);
}