using Aton.Domain.Models;

namespace Aton.Domain.Commands;

public class EditUserCommand : UserCommand
{
    public string Name { get; set; }
    public Gender? Gender { get; set; }
    public DateTime? Birtday { get; protected set; }
    public override bool IsValid()
    {
        throw new NotImplementedException();
    }
}