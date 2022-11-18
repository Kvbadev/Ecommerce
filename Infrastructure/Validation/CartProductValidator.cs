using Core;
using FluentValidation;

namespace Infrastructure.Validation;

public class CartProductValidator : AbstractValidator<CartProduct>
{
    public CartProductValidator()
    {
        RuleFor(x => x.ProductQuantity).GreaterThan(0);
    }
}
