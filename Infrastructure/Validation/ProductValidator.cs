using Core;
using FluentValidation;

namespace Infrastructure.Validation;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(x => x.Name).Length(1, 255);
        RuleFor(x => x.Price).GreaterThan(0M);
        RuleFor(x => x.Description).Length(1, 1024);
        RuleFor(x => x.Photos).Must(x => x.Count >= 1);
    }
}
