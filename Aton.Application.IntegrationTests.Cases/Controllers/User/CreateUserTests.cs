using System;
using System.Net;
using System.Threading.Tasks;
using Aton.Application.IntegrationTests.Cases.Utils;
using Aton.Application.IntegrationTests.Framework;
using Aton.Domain.Models;
using Aton.Services.Api.ViewModels;
using NUnit.Framework;

namespace Aton.Application.IntegrationTests.Cases.Controllers.User;

public class CreateUserTests : TestBase
{
    [Test]
    public async Task CreateUserTest()
    {
        var user = UserStorage.GenerateValidUserViewModel;
        await Client
            .Auth
                .LoginAsAdmin()
            //Create User
            .UserController
                .CreateUser(user)
                .Response
                    .AssertStatusCode(HttpStatusCode.Created)
                    .Json
                    .AssertThat<AspUserViewModel>(Is.Not.Null.And.Property(nameof(AspUserViewModel.Login)).EqualTo(user.Login))
                    .AssertThat<AspUserViewModel>(Is.Not.Null.And.Property(nameof(AspUserViewModel.Name)).EqualTo(user.Name))
                    .AssertThat<AspUserViewModel>(Is.Not.Null.And.Property(nameof(AspUserViewModel.Birthday)).EqualTo(user.Birthday))
                    .AssertThat<AspUserViewModel>(Is.Not.Null.And.Property(nameof(AspUserViewModel.Gender)).EqualTo(user.Gender))
                    .AssertThat<AspUserViewModel>(Is.Not.Null.And.Property(nameof(AspUserViewModel.CreatedAt)).EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(5.0)))
                    .AssertThat<AspUserViewModel>(Is.Not.Null.And.Property(nameof(AspUserViewModel.CreatedBy)).EqualTo("Admin"))
                    .AssertThat<AspUserViewModel>(Is.Not.Null.And.Property(nameof(AspUserViewModel.UpdatedAt)).EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(5.0)))
                    .AssertThat<AspUserViewModel>(Is.Not.Null.And.Property(nameof(AspUserViewModel.UpdatedBy)).EqualTo("Admin"))
            //Get this user
            .Client.UserController
                .GetUser(user.Login)
                .Response.Json
                    .AssertThat<AspUserViewModel>(Is.Not.Null.And.Property(nameof(AspUserViewModel.Login)).EqualTo(user.Login))
                    .AssertThat<AspUserViewModel>(Is.Not.Null.And.Property(nameof(AspUserViewModel.Name)).EqualTo(user.Name))
                    .AssertThat<AspUserViewModel>(Is.Not.Null.And.Property(nameof(AspUserViewModel.Birthday)).EqualTo(user.Birthday))
                    .AssertThat<AspUserViewModel>(Is.Not.Null.And.Property(nameof(AspUserViewModel.Gender)).EqualTo(user.Gender))
                    .AssertThat<AspUserViewModel>(Is.Not.Null.And.Property(nameof(AspUserViewModel.CreatedAt)).EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(5.0)))
                    .AssertThat<AspUserViewModel>(Is.Not.Null.And.Property(nameof(AspUserViewModel.CreatedBy)).EqualTo("Admin"))
                    .AssertThat<AspUserViewModel>(Is.Not.Null.And.Property(nameof(AspUserViewModel.UpdatedAt)).EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(5.0)))
                    .AssertThat<AspUserViewModel>(Is.Not.Null.And.Property(nameof(AspUserViewModel.UpdatedBy)).EqualTo("Admin"))
            //Create duplicate
            .Client.UserController
                .CreateUser(user)
                .Response
                    .AssertStatusCode(HttpStatusCode.BadRequest)
            .Client.Tasks.RunAsync();
    }
    
    [Test]
    public async Task CreateUserFromInvalidDataTest()
    {
        await Client
            .Auth
                .LoginAsAdmin()
            .UserController
                .CreateUser(UserStorage.InvalidUserViewModel)
                .Response
                    .AssertStatusCode(HttpStatusCode.BadRequest)
            .Client.Tasks.RunAsync();
    }
    
    
    [Test]
    public async Task DefaultUserCantCreateUserTest()
    {
        await Client
            .Auth
                .LoginAsDefaultUser()
            //Create User
            .UserController
                .CreateUser(UserStorage.GenerateValidUserViewModel)
                .Response
                    .AssertStatusCode(HttpStatusCode.Forbidden)
            .Client.Tasks.RunAsync();
    }
}