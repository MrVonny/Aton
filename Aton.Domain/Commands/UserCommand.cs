using Aton.Domain.Core.Commands;
using Aton.Domain.Models;

namespace Aton.Domain.Commands;

public abstract class UserCommand<T> : Command<T>, IUserCommand
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Gender? Gender { get; set; }
    public DateTime? Birthday { get; set; }
}

public interface IUserCommand
{
    public Guid Id { get;  set; }
    public string Name { get; set; }
    public Gender? Gender { get; set; }
    public DateTime? Birthday { get;  set; }
}
    