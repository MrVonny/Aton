using System;
using System.Net;
using System.Threading.Tasks;
using Aton.Application.IntegrationTests.Framework;
using Aton.Application.ViewModels;
using Aton.Domain.Models;
using NUnit.Framework;

namespace Aton.Application.IntegrationTests.Cases;

public class AuthorizationSmokeTest : TestBase
{

    private static CreateUserViewModel ValidUser => new CreateUserViewModel()
    {
        Name = "Козлов Кирилл",
        Login = "Trishu123",
        Password = "12345678",
        Admin = false,
        Gender = Gender.Male,
        Birthday = DateTime.Parse("1999-04-01")
    };
    
    [Test]
    public async Task ReturnsUnauthorizedWhenNoCredentials()
    {
        await Client
            .Auth
                .Clear()
            .UserController
                .GetMe()
                .Response
                    .AssertStatusCode(HttpStatusCode.Unauthorized)
            .Client.Tasks.RunAsync();
    }
    
    [Test]
    public async Task ReturnsOkForAdmin()
    {
        await Client
            .Auth
                .LoginAsAdmin()
            .UserController
                .GetActive()
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            .Client.Tasks.RunAsync();
    }
    
    [Test]
    public async Task ReturnsOkForUser()
    {
        await Client
            .Auth
                .LoginAsAdmin()
            .UserController
                .CreateUser(ValidUser)
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            .Client.Auth
                .FromUserNameAndPassword(ValidUser.Login, ValidUser.Password)
            .UserController
                .GetMe()
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            .Client.Tasks.RunAsync();
    }
    
    [Test]
    public async Task ReturnsForbiddenForUser()
    {
        await Client
            .Auth
                .LoginAsDefaultUser()
            .UserController
                .GetActive()
                .Response
                    .AssertStatusCode(HttpStatusCode.Forbidden)
            .Client.Tasks.RunAsync();
    }
}