using Aton.Application.Interfaces;
using Aton.Application.Services;
using Aton.Domain.CommandHandlers;
using Aton.Domain.Commands;
using Aton.Domain.Core.Bus;
using Aton.Domain.Core.Notifications;
using Aton.Domain.Intefaces;
using Aton.Domain.Models;
using Aton.Infrastructure.Bus;
using Aton.Infrastructure.Data.Contexts;
using Aton.Infrastructure.Data.Repository;
using Aton.Infrastructure.Data.UoW;
using Aton.Infrastructure.Identity.Managers;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aton.Infrastructure.IoC
{
    public static class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, InMemoryBus>();
            
            // Application
            services.AddScoped<IUserAppService, UserAppService>();

            // Domain - Events
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            // Domain - Commands
            services.AddScoped<IRequestHandler<CreateUserCommand, User>, UserCommandHandler>();
            services.AddScoped<IRequestHandler<EditUserCommand, User>, UserCommandHandler>();
            services.AddScoped<IRequestHandler<RevokeUserCommand, bool>, UserCommandHandler>();
            services.AddScoped<IRequestHandler<RestoreUserCommand, bool>, UserCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteUserCommand, bool>, UserCommandHandler>();

            // Infra - Data
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("Aton"));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            services.AddScoped<AccountManager>();
            services.AddScoped<SignInManager>();
        }
    }
}
