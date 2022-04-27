using Aton.Domain.Validations;

namespace Aton.Domain.Commands;

public class DeleteUserCommand : UserCommand<bool>
{
    public DeleteUserCommand(Guid guid)
    {
        Id = guid;
    }
    
    public override bool IsValid()
    {
        ValidationResult = new DeleteUserCommandValidation(this).Validate(this);
        return ValidationResult.IsValid;
    }
}