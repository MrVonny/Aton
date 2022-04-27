using System;
using System.Net;
using System.Threading.Tasks;
using Aton.Application.IntegrationTests.Cases.Utils;
using Aton.Application.IntegrationTests.Framework;
using Aton.Application.ViewModels;
using Aton.Domain.Models;
using NUnit.Framework;

namespace Aton.Application.IntegrationTests.Cases;

public class CreateUserTests : TestBase
{
   
    
    [Test]
    public async Task CreateUserTest()
    {
        await Client
            .Auth
                .LoginAsAdmin()
            //Create User
            .UserController
                .CreateUser(UserStorage.ValidUserViewModel)
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            //Get this user
            .Client.UserController
                .GetUser(UserStorage.ValidUserViewModel.Login)
                .Response
                    .AssertProperty("login", UserStorage.ValidUserViewModel.Login)
                    .AssertProperty("createdBy", "Admin")
                    .AssertProperty("name", UserStorage.ValidUserViewModel.Name)
            //Create duplicate
            .Client.UserController
                .CreateUser(UserStorage.ValidUserViewModel)
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