using Aton.Domain.Commands;
using Aton.Domain.Core.Bus;
using Aton.Domain.Core.Notifications;
using Aton.Domain.Intefaces;
using MediatR;

namespace Aton.Domain.CommandHandlers
{
    public class UserCommandHandler : CommandHandler,
        IRequestHandler<CreateUserCommand, bool>
        
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediatorHandler _bus;

        public UserCommandHandler(
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications,
            IUserRepository userRepository) : base(uow, bus, notifications)
        {
            _userRepository = userRepository;
            _bus = bus;
        }
        
        public Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
