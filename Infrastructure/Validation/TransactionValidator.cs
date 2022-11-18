using Core;
using FluentValidation;

namespace Infrastructure.Validation;

public class TransactionValidator : AbstractValidator<Transaction>
{
    public TransactionValidator()
    {
        RuleFor(x => x.Price).GreaterThanOrEqualTo(1M);
        RuleFor(x => x.Products).NotEmpty();
        RuleFor(x => x.IssuedAt).LessThanOrEqualTo(DateTime.UtcNow);
    }
}
