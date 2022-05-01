using Aton.Domain.Validations;

namespace Aton.Domain.Commands;

public class RevokeUserCommand : UserCommand<bool>
{
    public RevokeUserCommand(Guid guid, string revokedBy)
    {
        Id = guid;
        RevokedBy = revokedBy;
    }
    
    public string RevokedBy { get; set; }
    public override bool IsValid()
    {
        ValidationResult = new RevokeUserCommandValidation(this).Validate();
        return ValidationResult.IsValid;
    }
}