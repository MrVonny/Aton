using Aton.Domain.Core.Commands;
using Aton.Domain.Core.Events;

namespace Aton.Domain.Core.Bus;

public interface IMediatorHandler
{
    // Task SendCommand<T>(T command) where T : Command;
    Task<T> SendCommand<T>(Command<T> command) where T : class;
    Task<bool> SendCommand(Command<bool> command);
    Task RaiseEvent<T>(T @event) where T : Event;
}