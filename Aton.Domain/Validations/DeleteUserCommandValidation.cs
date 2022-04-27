using Aton.Domain.Commands;

namespace Aton.Domain.Validations;

public class DeleteUserCommandValidation : UserValidation<DeleteUserCommand>
{
    public DeleteUserCommandValidation(DeleteUserCommand command) : base(command)
    {
        ValidateId();
    }
}