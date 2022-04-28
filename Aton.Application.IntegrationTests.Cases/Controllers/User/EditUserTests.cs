using System;
using System.Net;
using System.Threading.Tasks;
using Aton.Application.IntegrationTests.Cases.Utils;
using Aton.Application.IntegrationTests.Framework;
using Aton.Services.Api.ViewModels;
using NUnit.Framework;

namespace Aton.Application.IntegrationTests.Cases.Controllers.User;

public class EditUserTests : TestBase
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
    
    [Test]
    public async Task EditInfoTest()
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
                .EditUserInfo(user.Login,  birthday: DateTime.Parse("13-05-2067"), name: "Михайлов Алексей")
                .Response
                    .AssertStatusCode(HttpStatusCode.BadRequest)
            .Client.UserController
                .GetMe()
                .Response.Json
                    .AssertThat<AspUserViewModel>(Is.Not.Null.And.Property(nameof(AspUserViewModel.Name)).EqualTo(user.Name))
                    .AssertThat<AspUserViewModel>(Is.Not.Null.And.Property(nameof(AspUserViewModel.Birthday)).EqualTo(user.Birthday))
            .Client.UserController
                .EditUserInfo(user.Login,  birthday: DateTime.Parse("13-05-1999"), name: "Михайлов Алексей")
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            .Client.UserController
                .GetMe()
                .Response.Json
                    .AssertThat<AspUserViewModel>(Is.Not.Null.And.Property(nameof(AspUserViewModel.Name)).EqualTo("Михайлов Алексей"))
                    .AssertThat<AspUserViewModel>(Is.Not.Null.And.Property(nameof(AspUserViewModel.Birthday)).EqualTo(DateTime.Parse("13-05-1999")))
            .Client.Tasks.RunAsync();
    }
}