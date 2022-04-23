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
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASP.NET HttpContext dependency
            services.AddHttpContextAccessor();
            // services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, InMemoryBus>();

            // ASP.NET Authorization Polices
            //services.AddSingleton<IAuthorizationHandler, ClaimsRequirementHandler>();

            // Application
            services.AddScoped<IUserAppService, UserAppService>();

            // Domain - Events
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            //services.AddScoped<INotificationHandler<CreateUserCommand>, CustomerEventHandler>();
            //services.AddScoped<INotificationHandler<CustomerUpdatedEvent>, CustomerEventHandler>();
            //services.AddScoped<INotificationHandler<CustomerRemovedEvent>, CustomerEventHandler>();

            // Domain - Commands
            services.AddScoped<IRequestHandler<CreateUserCommand, User>, UserCommandHandler>();
            services.AddScoped<IRequestHandler<EditUserCommand, User>, UserCommandHandler>();

            // Domain - 3rd parties
            // services.AddScoped<IHttpService, HttpService>();
            //services.AddScoped<IMailService, MailService>();

            // Infra - Data
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("Aton"));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Infra - Data EventSourcing
            //services.AddScoped<IEventStoreRepository, EventStoreSqlRepository>();
            //services.AddScoped<IEventStore, SqlEventStore>();

            // Infra - Identity Services
            //services.AddTransient<IEmailSender, AuthEmailMessageSender>();
            //services.AddTransient<ISmsSender, AuthSMSMessageSender>();

            // Infra - Identity
            //services.AddScoped<IUser, AspNetUser>();
            //services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddScoped<AccountManager>();
            services.AddScoped<SignInManager>();
        }
    }
}
