using Aton.Domain.Core.Bus;
using Aton.Domain.Core.Commands;
using Aton.Domain.Core.Events;
using MediatR;


namespace Aton.Infrastructure.Bus;

public class InMemoryBus : IMediatorHandler
{
    private readonly IMediator _mediator;

    public InMemoryBus(IMediator mediator)
    {
        _mediator = mediator;
    }

    // public Task SendCommand<T>(T command) where T : Command
    // {
    //     _mediator.Send(command);
    // }


    public async Task<T> SendCommand<T>(Command<T> command) where T : class
    {
        return await _mediator.Send((IRequest<T>)command);
    }

    public Task RaiseEvent<T>(T @event) where T : Event
    {
        // if (!@event.MessageType.Equals("DomainNotification"))
        //     _eventStore?.Save(@event);
    
        return _mediator.Publish(@event);
    }
}