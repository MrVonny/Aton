using System.Net;
using System.Threading.Tasks;
using Aton.Application.IntegrationTests.Framework;
using NUnit.Framework;

namespace Aton.Application.IntegrationTests.Cases;

public class AuthorizationSmokeTest : TestBase
{

    [Test]
    public async Task ReturnsUnauthorizedWhenNoCredentials()
    {
        await Client
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
                .LoginAsDefaultUser()
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