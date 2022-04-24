using Aton.Domain.Commands;

namespace Aton.Domain.Validations;

public class CreateUserCommandValidation : UserValidation<CreateUserCommand>
{
    public CreateUserCommandValidation(CreateUserCommand userCommand): base(userCommand)
    {
        ValidateGender();
        ValidateName();
        if(Object.Birthday != null)
            ValidateBirthDate();
    }

}