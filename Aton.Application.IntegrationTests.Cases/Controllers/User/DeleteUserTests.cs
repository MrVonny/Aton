using System;
using System.Net;
using System.Threading.Tasks;
using Aton.Application.IntegrationTests.Cases.Utils;
using Aton.Application.IntegrationTests.Framework;
using NUnit.Framework;

namespace Aton.Application.IntegrationTests.Cases.Controllers.User;

public class DeleteUserTests : TestBase
{
    [Test]
    public async Task SoftDeleteUserOk()
    {
        var user = UserStorage.GenerateValidUserViewModel;
        await Client
            .Auth
                .LoginAsAdmin()
            .UserController
                .CreateUser(user)
                .Response
                    .AssertStatusCode(HttpStatusCode.Created)
            .Client.UserController
                .DeleteUser(user.Login, true)
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            .Client.UserController
                .GetUser(user.Login)
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
                    .AssertProperty("revokedBy", "Admin")
                    .AssertProperty("revokedAt", typeof(DateTime),
                Is.Not.Null.And.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(15.0)))
            .Client.Tasks.RunAsync();
    }
    
    [Test]
    public async Task HardDeleteUserOk()
    {
        var user = UserStorage.GenerateValidUserViewModel;
        await Client
            .Auth
                .LoginAsAdmin()
            .UserController
                .CreateUser(user)
                .Response
                    .AssertStatusCode(HttpStatusCode.Created)
            .Client.UserController
                .DeleteUser(user.Login, false)
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            .Client.UserController
                .GetUser(user.Login)
                .Response
                    .AssertStatusCode(HttpStatusCode.BadRequest)
            .Client.Tasks.RunAsync();
    }
    
    [Test]
    public async Task DeleteNonExistentUserBadRequest()
    {
        await Client
            .Auth
                .LoginAsAdmin()
            .UserController
                .DeleteUser("RandomLogin", false)
                .Response
                    .AssertStatusCode(HttpStatusCode.BadRequest)
            .Client.Tasks.RunAsync();
    }
    
    [Test]
    public async Task DeletedUserCantUseAuth()
    {
        var user = UserStorage.GenerateValidUserViewModel;
        await Client
            .Auth
                .LoginAsAdmin()
            .UserController
                .CreateUser(user)
                .Response
                    .AssertStatusCode(HttpStatusCode.Created)
            .Client.UserController
                .DeleteUser(user.Login, true)
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            .Client.Auth
                .FromUserNameAndPassword(user.Login, user.Password)
            .UserController
                .GetMe()
                .Response
                    .AssertStatusCode(HttpStatusCode.Unauthorized)
            .Client.Tasks.RunAsync();
    }
    
    [Test]
    public async Task RestoreSoftDeletedUser()
    {
        var user = UserStorage.GenerateValidUserViewModel;
        await Client
            .Auth
                .LoginAsAdmin()
            .UserController
                .CreateUser(user)
                .Response
                    .AssertStatusCode(HttpStatusCode.Created)
            .Client.UserController
                .DeleteUser(user.Login, true)
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            .Client.UserController
                .Restore(user.Login)
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            .Client.Auth
                .FromUserNameAndPassword(user.Login, user.Password)
            .UserController
                .GetMe()
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            .Client.Tasks.RunAsync();
    }
    
    [Test]
    public async Task RestoreHardDeletedUser()
    {
        var user = UserStorage.GenerateValidUserViewModel;
        await Client
            .Auth
                .LoginAsAdmin()
            .UserController
                .CreateUser(user)
                .Response
                    .AssertStatusCode(HttpStatusCode.Created)
            .Client.UserController
                .DeleteUser(user.Login, false)
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            .Client.UserController
                .Restore(user.Login)
                .Response
                    .AssertStatusCode(HttpStatusCode.BadRequest)
            .Client.Tasks.RunAsync();
    }
}