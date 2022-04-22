using Aton.Domain.Commands;
using Aton.Domain.Core.Bus;
using Aton.Domain.Core.Notifications;
using Aton.Domain.Intefaces;
using Aton.Domain.Models;
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
        
        public Task<bool> Handle(CreateUserCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var user = new User(Guid.NewGuid(), message.Login, message.Name, message.Gender, message.Birthday);

            if (_userRepository.GetByLogin(user.Login) != null)
            {
                _bus.RaiseEvent(new DomainNotification(message.MessageType, "User with this login is already registered."));
                return Task.FromResult(false);
            }

            _userRepository.Add(user);

            Commit();

            return Task.FromResult(true);
        }
    }
}
