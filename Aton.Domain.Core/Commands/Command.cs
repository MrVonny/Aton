using FluentValidation.Results;
using Aton.Domain.Core.Events;
using MediatR;

namespace Aton.Domain.Core.Commands;

public abstract class Command : Message
{
    public DateTime Timestamp { get; private set; }
    public ValidationResult ValidationResult { get; set; }

    protected Command()
    {
        Timestamp = DateTime.Now;
    }

    public abstract bool IsValid();
}

public abstract class Command<T> : Command, IRequest<T>
{

}