using Aton.Domain.Validations;

namespace Aton.Domain.Commands;

public class RestoreUserCommand : UserCommand<bool>
{
    public RestoreUserCommand(Guid guid)
    {
        Id = guid;
    }
    
    public override bool IsValid()
    {
        ValidationResult = new RestoreUserCommandValidation(this).Validate();
        return ValidationResult.IsValid;
    }

}