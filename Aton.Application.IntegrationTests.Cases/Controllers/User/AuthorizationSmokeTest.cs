using System.Net;
using System.Threading.Tasks;
using Aton.Application.IntegrationTests.Cases.Utils;
using Aton.Application.IntegrationTests.Framework;
using NUnit.Framework;

namespace Aton.Application.IntegrationTests.Cases.Controllers.User;

public class AuthorizationSmokeTest : TestBase
{

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
                .CreateUser(UserStorage.ValidUserViewModel)
                .Response
                    .AssertStatusCode(HttpStatusCode.Created)
            .Client.Auth
                .FromUserNameAndPassword(UserStorage.ValidUserViewModel.Login, UserStorage.ValidUserViewModel.Password)
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