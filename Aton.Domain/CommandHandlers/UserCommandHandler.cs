using Aton.Domain.Commands;
using Aton.Domain.Core.Bus;
using Aton.Domain.Core.Notifications;
using Aton.Domain.Intefaces;
using Aton.Domain.Models;
using MediatR;

namespace Aton.Domain.CommandHandlers
{
    public class UserCommandHandler : CommandHandler,
        IRequestHandler<CreateUserCommand, User>,
        IRequestHandler<EditUserCommand, User>

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
        
        public async Task<User> Handle(CreateUserCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return default;
            }

            var user = new User(Guid.NewGuid(), message.Login, message.Name, message.Gender, message.Birthday);

            if (_userRepository.GetByLogin(user.Login) != null)
            {
                await _bus.RaiseEvent(new DomainNotification(message.MessageType, "User with this login is already registered."));
                return default;
            }

            _userRepository.Add(user);

            Commit();

            return user;
        }
        public async Task<User> Handle(EditUserCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return default;
            }

            var user = await _userRepository.GetById(message.Id);
            if (user == null)
            {
                await _bus.RaiseEvent(new DomainNotification(message.MessageType, $"User with Guid {message.Id} doesn't exists"));
                return default;
            }

            if (message.Birtday != null)
                user.Birthday = message.Birtday;
            if(message.Gender != null)
                user.Gender = message.Gender.Value;
            if (!string.IsNullOrEmpty(user.Name))
                user.Name = message.Name;
            
            Commit();

            return user;
        }
    }
}
