using System.Net;
using System.Threading.Tasks;
using Aton.Application.IntegrationTests.Cases.Utils;
using Aton.Application.IntegrationTests.Framework;
using NUnit.Framework;

namespace Aton.Application.IntegrationTests.Cases.Controllers.User;

public class AccountTests : TestBase
{
    [Test]
    public async Task ChangePasswordTest()
    {
        var user = UserStorage.GenerateValidUserViewModel;
        await Client
            .Auth
                .LoginAsAdmin()
            .UserController
                .CreateUser(user)
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            .Client.Auth
                .FromUserNameAndPassword(user.Login, user.Password)
            .UserController
                .EditUserPassword(user.Login, "Pass")
                .Response
                    .AssertStatusCode(HttpStatusCode.BadRequest)
            .Client.UserController
                .EditUserPassword(user.Login, "ValidPass123")
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            .Client.UserController
                .GetMe()
                .Response
                    .AssertStatusCode(HttpStatusCode.Unauthorized)
            .Client.Auth
                .FromUserNameAndPassword(user.Login, "ValidPass123")
            .UserController
                .GetMe()
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            .Client.Tasks.RunAsync();
    }
    
    [Test]
    public async Task ChangeLoginTest()
    {
        var user = UserStorage.GenerateValidUserViewModel;
        await Client
            .Auth
                .LoginAsAdmin()
            .UserController
                .CreateUser(user)
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            .Client.Auth
                .FromUserNameAndPassword(user.Login, user.Password)
            .UserController
                .EditUserLogin(user.Login, "s_#$")
                .Response
                    .AssertStatusCode(HttpStatusCode.BadRequest)
            .Client.UserController
                .EditUserLogin(user.Login, "TestLogin123")
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            .Client.UserController
                .GetMe()
                .Response
                    .AssertStatusCode(HttpStatusCode.Unauthorized)
            .Client.Auth
                .FromUserNameAndPassword("TestLogin123", user.Password)
            .UserController
                .GetMe()
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            .Client.Tasks.RunAsync();
    }
}