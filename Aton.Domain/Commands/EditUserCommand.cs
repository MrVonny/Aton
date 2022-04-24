using Aton.Domain.Models;
using Aton.Domain.Validations;

namespace Aton.Domain.Commands;

public class EditUserCommand : UserCommand
{
    public string UpdatedBy { get; set; }
    public EditUserCommand(Guid id, string name = null, Gender? gender = null, DateTime? birthday = null)
    {
        Id = id;
        Name = name;
        Gender = gender;
        Birthday = birthday;
    }
    public override bool IsValid()
    {
        ValidationResult = new EditUserCommandValidation(this).Validate(this);
        return ValidationResult.IsValid;
    }
}