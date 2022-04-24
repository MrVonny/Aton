using Aton.Domain.Commands;

namespace Aton.Domain.Validations;

public class RevokeUserCommandValidation : UserValidation<RevokeUserCommand>
{
    public RevokeUserCommandValidation(RevokeUserCommand command) : base(command)
    {
        ValidateId();
    }
}