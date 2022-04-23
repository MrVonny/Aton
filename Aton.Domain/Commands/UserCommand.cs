using Aton.Domain.Core.Commands;
using Aton.Domain.Models;
using MediatR;

namespace Aton.Domain.Commands;

public abstract class UserCommand : Command<User>
{
    public Guid Id { get; protected set; }
    public string Login { get; protected set; }
}