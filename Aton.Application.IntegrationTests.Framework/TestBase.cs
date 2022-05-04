using Aton.Application.IntegrationTests.Framework.Facades;
using Aton.Infrastructure.Data.Contexts;
using Aton.Infrastructure.Identity.Data;
using Aton.Infrastructure.Identity.Managers;
using Aton.Infrastructure.Identity.Models;
using Aton.Services.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Aton.Application.IntegrationTests.Framework;

[TestFixture]
public abstract class TestBase
{
    private readonly HttpClient _httpClient;
    private ClientFacade _client;

    protected ClientFacade Client => _client ??= new ClientFacade(_httpClient);
    protected ServiceProvider ServiceProvider = null;

    [SetUp]
    public virtual async Task SetUp()
    {
        using var scope = ServiceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        
        var accountManager = scopedServices.GetRequiredService<AccountManager>();
        await accountManager.CreateAsync(new CreateAccountViewModel(
            "TestUser", 
            "TestUser",
            false));
    }

    [TearDown]
    public virtual async Task TearDown()
    {
        using var scope = ServiceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        
        var applicationDb = scopedServices.GetRequiredService<ApplicationDbContext>();
        var accountDb = scopedServices.GetRequiredService<AccountDbContext>();
        
        await applicationDb.Database.EnsureDeletedAsync();
        await accountDb.Database.EnsureDeletedAsync();
    }
    
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
                    ServiceProvider = sp;
                });
            });

        _httpClient = application.CreateClient();
        
    }
}