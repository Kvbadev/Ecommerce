using Core;
using FluentValidation;

namespace Infrastructure.Validation;

public class AppUserValidator : AbstractValidator<AppUser>
{
    public AppUserValidator()
    {
        RuleFor(x => x.UserName).Length(4, 16);
        RuleFor(x => x.Firstname).Length(2, 20);
        RuleFor(x => x.Lastname).Length(2, 20);
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.CreationDate).LessThanOrEqualTo(DateTime.UtcNow);
    }

}