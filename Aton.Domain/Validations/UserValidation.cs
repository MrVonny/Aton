using Aton.Domain.Commands;
using FluentValidation;

namespace Aton.Domain.Validations;

public class UserValidation <T> : AbstractValidator<T> where T : IUserCommand
{
    protected readonly T Object;

    protected UserValidation(T value)
    {
        Object = value;
    }

    protected void ValidateName()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Please ensure you have entered the Name")
            .Length(2, 150).WithMessage("The Name must have between 2 and 150 characters")
            .Matches(@"[ЁёА-яa-zA-Z ]").WithMessage("Only Latin and Cyrillic letters are allowed");
    }

    protected void ValidateBirthDate()
    {
        RuleFor(c => c.Birthday)
            .NotEmpty()
            .Must(HaveRealAge)
            .WithMessage("The user must have real Date of Birth");
    }
    
    protected void ValidateGender()
    {
        RuleFor(c => c.Gender)
            .NotEmpty();
    }

    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }

    private static bool HaveRealAge(DateTime? birthDate)
    {
        return birthDate <= DateTime.Now && birthDate >= DateTime.Now.AddYears(-100);
    }
}