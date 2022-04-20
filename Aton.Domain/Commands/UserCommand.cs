using Aton.Domain.Core.Commands;

namespace Aton.Domain.Commands;

public abstract class UserCommand : Command
{
    public Guid Id { get; protected set; }

    public string Name { get; protected set; }

    public DateTime BirthDate { get; protected set; }
}