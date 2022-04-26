using Aton.Domain.Commands;

namespace Aton.Domain.Validations;

public class RestoreUserCommandValidation : UserValidation<RestoreUserCommand>
{
    public RestoreUserCommandValidation(RestoreUserCommand command) : base(command)
    {
        ValidateId();
    }
}