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
        IRequestHandler<EditUserCommand, User>,
        IRequestHandler<RevokeUserCommand, bool>,
        IRequestHandler<RestoreUserCommand, bool>,
        IRequestHandler<DeleteUserCommand, bool>

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

            var user = new User(Guid.NewGuid(), message.Name, message.Gender!.Value, message.Birthday);
            user.CreatedBy = message.CreatedBy;
            user.UpdatedBy = message.CreatedBy;
            
            if (await _userRepository.GetById(user.Id) != null)
            {
                await _bus.RaiseEvent(new DomainNotification(message.MessageType, "User is already registered."));
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

            user.UpdatedBy = message.UpdatedBy;

            if (message.Birthday != null)
                user.Birthday = message.Birthday;
            if(message.Gender != null)
                user.Gender = message.Gender.Value;
            if (!string.IsNullOrEmpty(user.Name))
                user.Name = message.Name;
            
            Commit();

            return user;
        }
        
        public async Task<bool> Handle(RevokeUserCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return false;
            }

            var user = await _userRepository.GetById(message.Id);
            if (user == null)
            {
                await _bus.RaiseEvent(new DomainNotification(message.MessageType, $"User with Guid {message.Id} doesn't exists"));
                return false;
            }

            user.RevokedBy = message.RevokedBy;
            user.RevokedAt = DateTime.Now;

            Commit();

            return true;
        }

        public async Task<bool> Handle(RestoreUserCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return false;
            }

            var user = await _userRepository.GetById(message.Id);
            if (user == null)
            {
                await _bus.RaiseEvent(new DomainNotification(message.MessageType, $"User with Guid {message.Id} doesn't exists"));
                return false;
            }

            user.RevokedBy = null;
            user.RevokedAt = null;

            Commit();

            return true;
        }

        public async Task<bool> Handle(DeleteUserCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return false;
            }

            var user = await _userRepository.GetById(message.Id);
            if (user == null)
            {
                await _bus.RaiseEvent(new DomainNotification(message.MessageType, $"User with Guid {message.Id} doesn't exists"));
                return false;
            }

            _userRepository.Remove(message.Id);

            Commit();

            return true;
        }
    }
}
