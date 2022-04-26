using Aton.Domain.Models;
using Aton.Domain.Validations;

namespace Aton.Domain.Commands;

public class CreateUserCommand : UserCommand<User>
{
    public string CreatedBy { get; set; }
    public CreateUserCommand(string name, Gender gender , DateTime? birthday = null)
    {
        Name = name;
        Gender = gender;
        Birthday = birthday;
    }
    public override bool IsValid()
    {
        ValidationResult = new CreateUserCommandValidation(this).Validate(this);
        return ValidationResult.IsValid;
    }
}