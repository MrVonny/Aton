using FluentValidation;

namespace Aton.Infrastructure.Identity.Validators;

public class LoginValidator : AbstractValidator<string>
{
    public LoginValidator()
    {
        RuleFor(login => login)
            .NotEmpty().WithMessage("Please ensure you have entered the login")
            .Length(5, 50).WithMessage("The login must have between 5 and 50 characters")
            .Matches(@"^[a-zA-Z0-9]*$").WithMessage("Only Latin letters and numbers are allowed");
    }
}