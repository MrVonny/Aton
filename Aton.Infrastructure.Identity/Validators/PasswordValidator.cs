using FluentValidation;

namespace Aton.Infrastructure.Identity.Validators;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(pass => pass)
            .NotEmpty().WithMessage("Please ensure you have entered the password")
            .Length(8, 50).WithMessage("The password must have between 8 and 50 characters")
            .Matches(@"^[a-zA-Z0-9]*$").WithMessage("Only Latin letters and numbers are allowed");
    }
}