using Aton.Application.IntegrationTests.Framework.Facades;
using Aton.Application.IntegrationTests.Framework.Wrappers;
using Aton.Infrastructure.Data.Contexts;
using Aton.Infrastructure.Identity.Data;
using Aton.Infrastructure.Identity.Managers;
using Aton.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Aton.Application.IntegrationTests.Framework;

[TestFixture]
public abstract class TestBase
{
    private readonly HttpClient _client;

    protected ClientFacade Client => new ClientFacade(_client);

    protected TestBase()
    {
        var randomDbName = Guid.NewGuid().ToString();
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(async services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                             typeof(DbContextOptions<ApplicationDbContext>));
                    services.Remove(descriptor);
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase(randomDbName);
                    });
                    
                    descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                             typeof(DbContextOptions<AccountDbContext>));
                    services.Remove(descriptor);
                    services.AddDbContext<AccountDbContext>(options =>
                    {
                        options.UseInMemoryDatabase(randomDbName);
                    });

                    var sp = services.BuildServiceProvider();

                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var accountManager = scopedServices.GetRequiredService<AccountManager>();
                        try
                        {
                            await accountManager.CreateAsync(new Account("TestUser", "TestUser", false));
                        }
                        catch (Exception ex)
                        {
                            // logger.LogError(ex, "An error occurred seeding the " +
                            //                     "database with test messages. Error: {Message}", ex.Message);
                        }
                    }
                });
            });

        _client = application.CreateClient();
        
    }
}