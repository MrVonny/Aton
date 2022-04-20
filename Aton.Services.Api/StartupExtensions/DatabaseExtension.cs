using Aton.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Aton.Services.Api.StartupExtensions
{
    public static class DatabaseExtension
    {
        public static IServiceCollection AddCustomizedDatabase(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
           services.AddDbContext<ApplicationDbContext>(options =>
           {
               options.UseInMemoryDatabase("Aton");
               if (!env.IsProduction())
               {
                   options.EnableDetailedErrors();
                   options.EnableSensitiveDataLogging();
               }
           });

           return services;
        }
    }
}
