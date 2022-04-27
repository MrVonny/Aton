﻿using Aton.Application.IntegrationTests.Framework.Facades;
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
    private readonly WebApplicationFactory<Program> _application;
    private HttpClient _client;

    public ClientFacade Client => new ClientFacade(_client);

    public TestBase()
    {
        _application = new WebApplicationFactory<Program>()
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
                        options.UseInMemoryDatabase("AtonDbForTesting");
                    });
                    
                    descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                             typeof(DbContextOptions<AccountDbContext>));
                    services.Remove(descriptor);
                    services.AddDbContext<AccountDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("IdentityDbForTesting");
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

        _client = _application.CreateClient();
        
    }
}