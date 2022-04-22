using Aton.Domain.Core.Commands;
using Aton.Domain.Models;

namespace Aton.Domain.Commands;

public abstract class UserCommand : Command
{
    public Guid Id { get; protected set; }
    public string Login { get; protected set; }
}