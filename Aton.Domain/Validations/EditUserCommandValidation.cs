using Aton.Domain.Commands;

namespace Aton.Domain.Validations;

public class EditUserCommandValidation : UserValidation<EditUserCommand>
{
    public EditUserCommandValidation(EditUserCommand command) : base(command)
    {
        ValidateId();
        if(Object.Gender != null)
            ValidateGender();
        if(Object.Name != null)
            ValidateName();
        if(Object.Birthday != null)
            ValidateBirthDate();
    }
}